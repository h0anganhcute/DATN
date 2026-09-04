using UnityEngine;

namespace Unity.FPS.Game
{
    public static class PlayerRespawnManager
    {
        public static bool HasRespawnData { get; private set; }
        
        public static Vector3 DeathPosition { get; private set; }
        public static Quaternion DeathRotation { get; private set; }
        public static float DeathCameraVerticalAngle { get; private set; }

        public static void SaveDeathState( Vector3 position, Quaternion rotation, float cameraVerticalAngle)
        {
            
            DeathPosition = position;
            DeathRotation = rotation;
            DeathCameraVerticalAngle = cameraVerticalAngle;
            HasRespawnData = true;
        }

        public static void ConsumeRespawnData()
        {
            HasRespawnData = false;
        }

        public static void ResetRespawnData()
        {
            HasRespawnData = false;
            
            DeathPosition = Vector3.zero;
            DeathRotation = Quaternion.identity;
            DeathCameraVerticalAngle = 0f;
        }
    }
}
