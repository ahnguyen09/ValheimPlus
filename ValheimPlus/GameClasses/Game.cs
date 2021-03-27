﻿using System;
using HarmonyLib;
using ValheimPlus.RPC;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Sync server config to clients
    /// </summary>
    [HarmonyPatch(typeof(Game), "Start")]
    public static class Game_Start_Patch
    {
        private static void Prefix()
        {
            ZRoutedRpc.instance.Register("VPlusConfigSync", new Action<long, ZPackage>(VPlusConfigSync.RPC_VPlusConfigSync)); //Config Sync
            ZRoutedRpc.instance.Register("VPlusMapSync", new Action<long, ZPackage>(VPlusMapSync.RPC_VPlusMapSync)); //Map Sync
            ZRoutedRpc.instance.Register("VPlusAck", new Action<long>(VPlusAck.RPC_VPlusAck)); //Ack
        }
    }


    /// <summary>
    /// Alter game difficulty damage scale
    /// </summary>
    [HarmonyPatch(typeof(Game), "GetDifficultyDamageScale")]
    public static class Game_GetDifficultyDamageScale_Patch
    {
        private static bool Prefix(ref Game __instance, ref Vector3 pos, ref float __result)
        {
            if (Configuration.Current.Game.IsEnabled)
            {
                int playerDifficulty = __instance.GetPlayerDifficulty(pos);
                __result = 1f + (float)(playerDifficulty - 1) * Configuration.Current.Game.gameDifficultyDamageScale;
                return false;
            }

            return true;
        }
    }


    /// <summary>
    /// Disable the "i have arrived" message on spawn.
    /// </summary>
    [HarmonyPatch(typeof(Game), "UpdateRespawn")]
    public static class Game_UpdateRespawn_Patch
    {
        private static void Prefix(ref Game __instance, float dt)
        {
            if(Configuration.Current.Player.IsEnabled && !Configuration.Current.Player.iHaveArrivedOnSpawn)
                __instance.m_firstSpawn = false;
        }
    }

    /// <summary>
    /// Alter game difficulty health scale
    /// </summary>
    [HarmonyPatch(typeof(Game), "GetDifficultyHealthScale")]
    public static class Game_GetDifficultyHealthScale_Patch
    {
        private static bool Prefix(ref Game __instance, ref Vector3 pos, ref float __result)
        {
            if (Configuration.Current.Game.IsEnabled)
            {
                int playerDifficulty = __instance.GetPlayerDifficulty(pos);
                __result = 1f + (float)(playerDifficulty - 1) * Configuration.Current.Game.gameDifficultyHealthScale;
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Alter player difficulty scale
    /// </summary>
    [HarmonyPatch(typeof(Game), "GetPlayerDifficulty")]
    public static class Game_GetPlayerDifficulty_Patch
    {
        private static bool Prefix(ref Game __instance, ref Vector3 pos, ref int __result)
        {
            if (Configuration.Current.Game.IsEnabled)
            {
                if (Configuration.Current.Game.setFixedPlayerCountTo > 0)
                {
                    __result = Configuration.Current.Game.setFixedPlayerCountTo + Configuration.Current.Game.extraPlayerCountNearby;
                    return false;
                }

                int num = Player.GetPlayersInRangeXZ(pos, Configuration.Current.Game.difficultyScaleRange);

                if (num < 1)
                {
                    num = 1;
                }

                __result = num + Configuration.Current.Game.extraPlayerCountNearby;
                return false;
            }

            return true;
        }
    }

    /*
    [HarmonyPatch(typeof(Game), "RequestRespawn")]
    public static class Game_RequestRespawn_Patch
    {
        private static void Postfix(ref Game __instance, ref float delay)
        {
            //Player.m_localPlayer.IsSafeInHome() does not seem to work
            if (true)
            {
                Player.m_localPlayer.m_seman.AddStatusEffect("Rested", true);
            }
        }
    }
    */
}