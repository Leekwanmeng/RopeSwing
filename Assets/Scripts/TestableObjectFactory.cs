using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public static class TestableObjectFactory
{
    public static T Create<T>()
    {
        return (T) FormatterServices.GetUninitializedObject(typeof(T));
    }
}
