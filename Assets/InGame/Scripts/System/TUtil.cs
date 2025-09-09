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
                return (T)Convert.ChangeType(data[key], typeof(T)); // 타입에 맞게 변환
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error converting {key}: {ex.Message}");
            }
        }
        else
        {
            // 키가 없을 경우 경고 메시지 출력
            Debug.LogWarning($"{key} 키를 찾지 못했습니다");
        }
        return default(T); // 기본값 반환 (값이 없거나 변환 실패 시)
    }
}
