using System;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public static class TrueRandom
{
    // AES-CTR 관련 필드
    private static AesManaged aes;
    private static byte[] counter;
    private static ICryptoTransform encryptor;
    
    // 엔트로피 수집기
    private static readonly List<byte> EntropyPool = new ();
    private static float nextReseedTime = 0f;
    private const float reseedInterval = 1f; // 1초마다 재시드
    
    // 초기화: 실행 시점에 호출
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        // AES 세팅
        aes = new AesManaged
        {
            Mode = CipherMode.ECB,
            Padding = PaddingMode.None,
            KeySize = 256
        };
        counter = new byte[16];
        var key = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);
        rng.GetBytes(counter);
        encryptor = aes.CreateEncryptor(key, new byte[16]);
        nextReseedTime = Time.time + reseedInterval;
    }
    
    // 각 프레임 엔트로피 수집
    public static void CollectEntropy()
    {
        // deltaTime & frameCount
        float dt = Time.deltaTime;
        uint fc = (uint)Time.frameCount;
        byte[] b1 = BitConverter.GetBytes(dt);
        byte[] b2 = BitConverter.GetBytes(fc);
        EntropyPool.AddRange(b1);
        EntropyPool.AddRange(b2);
        
        // 마우스 위치
        Vector2 m = Input.mousePosition;
        byte[] b3 = BitConverter.GetBytes(m.x);
        byte[] b4 = BitConverter.GetBytes(m.y);
        EntropyPool.AddRange(b3);
        EntropyPool.AddRange(b4);
    }
    
    // 재시드: AES 키와 카운터 갱신
    private static void ReseedIfNeeded()
    {
        if (Time.time < nextReseedTime || EntropyPool.Count < 32)
            return;

        using (var sha = SHA256.Create())
        {
            byte[] hash = sha.ComputeHash(EntropyPool.ToArray());
            aes.Key = hash;
            Array.Copy(hash, 0, counter, 0, 16);
        }
        encryptor = aes.CreateEncryptor(aes.Key, new byte[16]);
        EntropyPool.Clear();
        nextReseedTime = Time.time + reseedInterval;
    }
    
    // 바이트 스트림 생성(AES-CTR)
    private static byte[] GenerateBytes(int count)
    {
        ReseedIfNeeded();
        
        byte[] result = new byte[count];
        byte[] block = new byte[16];
        int offset = 0;
        while (offset < count)
        {
            encryptor.TransformBlock(counter, 0, 16, block, 0);
            IncrementCounter();
            
            int chunk = Math.Min(16, count - offset);
            Array.Copy(block, 0, result, offset, chunk);
            offset += chunk;
        }
        return result;
    }

    private static void IncrementCounter()
    {
        for (int i = 15; i >= 0; i--)
        {
            if (++counter[i] != 0) break;
        }
    }
    
    // 범위 내 정수 반환
    public static int NextInt(int minInclusive, int maxExclusive)
    {
        if (minInclusive > maxExclusive)
            throw new ArgumentException($"minInclusive({minInclusive}) > maxExclusive({maxExclusive})");

        if (minInclusive == maxExclusive)
            return minInclusive;

        long range = (long)maxExclusive - minInclusive;
        int bitsNeeded = (int)Math.Ceiling(Math.Log(range, 2));
        int bytesNeeded = Math.Max(1, (bitsNeeded + 7) / 8);

        while (true)
        {
            byte[] rand = GenerateBytes(bytesNeeded);
            if (rand == null || rand.Length < bytesNeeded)
            {
                Debug.LogWarning($"GenerateBytes 반환 오류: 필요={bytesNeeded}, 실제={rand?.Length ?? 0}");
                continue;  // 재시도
            }

            long value = 0;
            for (int i = 0; i < bytesNeeded; i++)
                value |= ((long)rand[i] << (8 * i));

            long mask = ((1L << bitsNeeded) - 1);
            long candidate = value & mask;
            if (candidate < range)
                return minInclusive + (int)candidate;
            // else 재시도
        }
    }
    
    // 실수 (0, 1)
    public static float NextFloat()
    {
        uint v = (uint)BitConverter.ToInt32(GenerateBytes(4), 0);
        return v / (uint.MaxValue + 1f);
    }
    
    // Boolean
    public static bool NextBool()
    {
        return (GenerateBytes(1)[0] & 1) == 1;
    }
}
