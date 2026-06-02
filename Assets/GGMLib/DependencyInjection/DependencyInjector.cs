using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GGMLib.DependencyInjection
{
    [DefaultExecutionOrder(-10)] //다른 녀석들보다 먼저 실행되게 한다.
    public class DependencyInjector : MonoBehaviour
    {
        private const BindingFlags _bindingFlags 
            = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        //주입할 의존성들을 넣어두는 사전.
        private readonly Dictionary<Type, object> _registry = new Dictionary<Type, object>();

        private void Awake()
        {
            IEnumerable<IDependencyProvider> providers = FindMonoBehaviours().OfType<IDependencyProvider>();

            foreach (IDependencyProvider provider in providers)
            {
                RegisterProvider(provider);
            }

            IEnumerable<MonoBehaviour> injectables = FindMonoBehaviours().Where(IsInjectable);
            foreach (MonoBehaviour injectableMono in injectables)
            {
                Inject(injectableMono);
            }
        }

        private void Inject(MonoBehaviour injectableMono)
        {
            Type type = injectableMono.GetType();
            //해당 타입에서 Inject가 붙어있는 필드를 전부 가져온다.
            IEnumerable<FieldInfo> injectableFields = type.GetFields(_bindingFlags)
                .Where(field => Attribute.IsDefined(field, typeof(InjectAttribute)));
            
            //주입이 필요한 필드에 값을 주입해준다.
            foreach (FieldInfo field in injectableFields)
            {
                Type fieldType = field.FieldType;
                object injectInstance = Resolve(fieldType); //해당 필드타입에 맞는 Provider를 찾는다.
                Debug.Assert(injectInstance != null, $"주입할 인스턴스를 찾지 못했습니다. {field.Name}");
                
                field.SetValue(injectableMono, injectInstance); //찾아온 값을 셋팅해준다.
            }
            //매서드에 주입하기
            IEnumerable<MethodInfo> injectableMethods = type.GetMethods(_bindingFlags)
                .Where(method => Attribute.IsDefined(method, typeof(InjectAttribute)));

            foreach (MethodInfo method in injectableMethods)
            {
                //해당 매서드를 실행하는 데 필요한 모든 파라메터를 다가져와야 해.
                Type[] requiredParams = method.GetParameters().Select(p => p.ParameterType).ToArray();
                object[] parameters = requiredParams.Select(Resolve).ToArray(); //각 파라메터를 찾아서 배열로 만든다.
                method.Invoke(injectableMono, parameters); //이렇게 콜해준다.
            }
        }

        private object Resolve(Type fieldType)
        {
            _registry.TryGetValue(fieldType, out object instance);
            return instance;
        }

        private bool IsInjectable(MonoBehaviour mono)
        {
            MemberInfo[] members = mono.GetType().GetMembers(_bindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }

        private void RegisterProvider(IDependencyProvider provider)
        {
            //클래스 그 자체를 Provide하는 경우.
            if (Attribute.IsDefined(provider.GetType(), typeof(ProvideAttribute)))
            {
                _registry.Add(provider.GetType(), provider);
                return;
            }
            
            //해당 클래스에서 Provide되고 있는 매서드가 있는지를 찾아서 그걸 실행해서 값을 가져온다.
            MethodInfo[] methods = provider.GetType().GetMethods(_bindingFlags);

            foreach (MethodInfo method in methods)
            {
                if(!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;
                
                Type returnType = method.ReturnType;
                object providedInstance = method.Invoke(provider, null);
                Debug.Assert(providedInstance != null, $"Provided instance is null : {method.Name}");
                
                _registry.Add(returnType, providedInstance);
            }
            
        }

        private static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        }
    }
}