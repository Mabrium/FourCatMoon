using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TUtil
{
    public static T GetValue<T>(Dictionary<string, object> data, string key)
    {
        if (data.ContainsKey(key))
        {
            try
            {
                return (T)Convert.ChangeType(data[key], typeof(T)); // Ÿ�Կ� �°� ��ȯ
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error converting {key}: {ex.Message}");
            }
        }
        else
        {
            // Ű�� ���� ��� ��� �޽��� ���
            Debug.LogWarning($"Key {key} not found in Firestore data.");
        }
        return default(T); // �⺻�� ��ȯ (���� ���ų� ��ȯ ���� ��)
    }
}
