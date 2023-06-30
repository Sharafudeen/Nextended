﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Nextended.Core.Helper;
using Nextended.Core.Types;

namespace Nextended.Core.Extensions
{
    public static class TypeExtensions
    {
        
        /// <summary>
        /// Gibt an ob der Typ eine BaseId ist
        /// </summary>
        public static bool IsBaseId(this Type modelType)
        {
            return GetBaseIdBaseType(modelType) != null;
        }

        /// <summary>
        /// Gibt an ob der Typ eine BaseId ist
        /// </summary>
        public static Type GetBaseIdBaseType(this Type modelType)
        {
            Type baseType = modelType.BaseType;
            while (baseType != null && baseType != typeof(object))
            {
                if (baseType.IsGenericType
                    && baseType.GetGenericTypeDefinition() == typeof(BaseId<,>))
                    return baseType;
                baseType = baseType.BaseType;
            }

            return null;
        }

        public static PropertyInfo GetPropertyIgnoreCase(this Type type, string propertyName)
        {
            var typeList = new List<Type> { type };

            if (type.IsInterface)
                typeList.AddRange(type.GetInterfaces());

            return typeList
                .Select(interfaceType => interfaceType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance))
                .FirstOrDefault(property => property != null);
        }

        public static bool IsCollection(this Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>) || type.GetInterfaces().Any(IsCollection);


        public static bool IsNullableEnum(this Type t)
            => Nullable.GetUnderlyingType(t) is { IsEnum: true };

        
        public static bool IsExpression(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Expression<>);

        public static bool IsNullableType(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public static bool IsNullable(this Type type)
        {
            Check.NotNull(() => type);
            return !type.IsValueType || IsNullableType(type);
        }

        public static bool IsNullableOf<T>(this Type type) 
            => IsNullableType(type) && type.GetGenericArguments().FirstOrDefault() == typeof(T);

        public static bool IsString(this Type input) 
            => input == typeof(string);

        public static bool IsDecimal(this Type input) 
            => input == typeof(decimal);

        public static bool IsInt(this Type input) 
            => input == typeof(int);

        public static bool IsDateTime(this Type input) 
            => input == typeof(DateTime);

        public static bool IsBool(this Type input) 
            => input == typeof(bool);

        public static bool IsNullableDecimal(this Type input) 
            => input == typeof(decimal?);

        public static bool IsNullableInt(this Type input) 
            => input == typeof(int?);

        public static bool IsNullableDateTime(this Type input) 
            => input == typeof(DateTime?);

        public static bool IsNullableBool(this Type input) 
            => input == typeof(bool?);

        public static bool IsType(this Type input) 
            => input == typeof(Type);

        public static bool IsIEnumerable(this Type input) 
            => typeof(IEnumerable).IsAssignableFrom(input);

        public static bool IsEnumerableOrArray(this Type type) 
            => typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string) || type.IsArray;

        public static bool IsEnumerable(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || type.GetInterfaces().Any(IsCollection);


        public static bool IsIList(this Type input) 
            => typeof(IList).IsAssignableFrom(input);

        public static bool IsNotString(this Type input) 
            => input != typeof(string);

        public static bool IsNotDecimal(this Type input) 
            => input != typeof(decimal);

        public static bool IsNotInt(this Type input) 
            => input != typeof(int);

        public static bool IsNotDateTime(this Type input) 
            => input != typeof(DateTime);

        public static bool IsNotBool(this Type input) 
            => input != typeof(bool);

        public static bool IsNotNullableDecimal(this Type input) 
            => input != typeof(decimal?);

        public static bool IsNotNullableInt(this Type input) 
            => input != typeof(int?);

        public static bool IsNotNullableDateTime(this Type input) 
            => input != typeof(DateTime?);

        public static bool IsNotNullableBool(this Type input) 
            => input != typeof(bool?);

        public static bool IsNotType(this Type input) 
            => input != typeof(Type);

        public static bool IsNotIEnumerable(this Type input) 
            => !typeof(IEnumerable).IsAssignableFrom(input);

        public static bool IsNotIList(this Type input) 
            => !typeof(IList).IsAssignableFrom(input);

        public static bool IsStruct(this Type t) 
            => t.IsValueType && !t.IsPrimitive && !t.IsEnum && t != typeof(ValueType) && (!t.IsGenericType || t.GetGenericTypeDefinition() != typeof(Nullable<>));

        public static bool IsReadOnlyStruct(this Type t) 
            => IsStruct(t) && t.GetProperties().All(prop => !prop.CanWrite);

        public static object CreateInstance(this Type input) 
            => ReflectionHelper.CreateInstance(input);

        public static T CreateInstance<T>(this Type input) 
            => ReflectionHelper.CreateInstance<T>();

        public static T CreateInstance<T>(this Type input, params object[] args) 
            => (T)Activator.CreateInstance(input, args);

        public static bool IsSubclassOfInterfaceOf<TInterface>(this Type toCheck) 
            => IsSubclassOfInterfaceOf(toCheck, typeof(TInterface));

        public static bool IsKeyValuePair(this Type t)
        {
            if (t.IsGenericType)
            {
                var def = t.GetGenericTypeDefinition();
                return def == typeof(KeyValuePair<,>);
            }
            return false;
        }
        
        public static bool IsTupleOrValueTuple(this Type t)
            => IsTuple(t) || IsValueTuple(t);
        
        public static bool IsTuple(this Type t)
        {
            if (t.IsGenericType)
            {
                var def = t.GetGenericTypeDefinition();
                return def == typeof(Tuple<>) ||
                       def == typeof(Tuple<,>) ||
                       def == typeof(Tuple<,,>) ||
                       def == typeof(Tuple<,,,>) ||
                       def == typeof(Tuple<,,,,>) ||
                       def == typeof(Tuple<,,,,,>) ||
                       def == typeof(Tuple<,,,,,,>) ||
                       def == typeof(Tuple<,,,,,,,>);
            }
            return false;
        }

        public static bool IsValueTuple(this Type t)
        {
            if (t.IsGenericType)
            {
                var def = t.GetGenericTypeDefinition();
                return def == typeof(ValueTuple<>) ||
                       def == typeof(ValueTuple<,>) ||
                       def == typeof(ValueTuple<,,>) ||
                       def == typeof(ValueTuple<,,,>) ||
                       def == typeof(ValueTuple<,,,,>) ||
                       def == typeof(ValueTuple<,,,,,>) ||
                       def == typeof(ValueTuple<,,,,,,>) ||
                       def == typeof(ValueTuple<,,,,,,,>);
            }
            return false;
        }

        public static bool IsSubclassOfInterfaceOf(this Type toCheck, Type interfaceType)
        {
            var interfaces = toCheck.GetInterfaces();

            foreach (var interfaceOfCheck in interfaces)
            {
                if (interfaceOfCheck.GetTypeInfo().IsGenericType)
                {
                    if (interfaceOfCheck.GetGenericTypeDefinition() == interfaceType)
                        return true;
                    
                }
                else if (interfaceOfCheck == interfaceType)
                    return true;
            }

            return false;
        }
    }
}