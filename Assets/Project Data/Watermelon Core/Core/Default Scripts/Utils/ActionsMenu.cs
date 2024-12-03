using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

// Menues customization tutorial
// https://hugecalf-studios.github.io/unity-lessons/lessons/editor/menuitem/#:~:text=To%20disable%20a%20menu%20item,return%20false%20from%20this%20method.

namespace Watermelon
{
    public static class ActionsMenu
    {
#if UNITY_EDITOR

        #region Save Management

        [MenuItem("Actions/Remove Save", priority = 1)]
        private static void RemoveSave()
        {
            PlayerPrefs.DeleteAll();
            SaveController.DeleteSaveFile();
        }

        [MenuItem("Actions/Remove Save", true)]
        private static bool RemoveSaveValidation()
        {
            return !Application.isPlaying;
        }

        #endregion

        #region Currencies

        //[MenuItem("Actions/Lots of Money", priority = 21)]
        //private static void LotsOfMoney()
        //{
        //    CurrenciesController.Set(CurrencyType.Coin, 2000000);
        //}

        //[MenuItem("Actions/Lots of Money", true)]
        //private static bool LotsOfMoneyValidation()
        //{
        //    return Application.isPlaying;
        //}

        //[MenuItem("Actions/No Money", priority = 22)]
        //private static void NoMoney()
        //{
        //    CurrenciesController.Set(CurrencyType.Coin, 0);
        //}

        //[MenuItem("Actions/No Money", true)]
        //private static bool NoMoneyValidation()
        //{
        //    return Application.isPlaying;
        //}

        //[MenuItem("Actions/Lots of Gems", priority = 23)]
        //private static void LotsOfGems()
        //{
        //    CurrenciesController.Set(CurrencyType.Gems, 100);
        //}

        //[MenuItem("Actions/Lots of Gems", true)]
        //private static bool LotsOfGemsValidation()
        //{
        //    return Application.isPlaying;
        //}

        //[MenuItem("Actions/No Gems", priority = 24)]
        //private static void NoGems()
        //{
        //    CurrenciesController.Set(CurrencyType.Gems, 0);
        //}

        //[MenuItem("Actions/No Gems", true)]
        //private static bool NoGemsValidation()
        //{
        //    return Application.isPlaying;
        //}

        #endregion

        #region Levels and Scenes

        //[MenuItem("Actions/Prev Level (menu) [P]", priority = 71)]
        //public static void PrevLevel()
        //{
        //    LevelController.PrevLevelDev();
        //}

        //[MenuItem("Actions/Prev Level (menu) [P]", true)]
        //private static bool PrevLevelValidation()
        //{
        //    return Application.isPlaying;
        //}

        //[MenuItem("Actions/Next Level (menu) [N]", priority = 72)]
        //public static void NextLevel()
        //{
        //    LevelController.NextLevelDev();
        //}

        //[MenuItem("Actions/Next Level (menu) [N]", true)]
        //private static bool NextLevelValidation()
        //{
        //    return Application.isPlaying;
        //}

        [MenuItem("Actions/Game Scene", priority = 100)]
        private static void GameScene()
        {
            EditorSceneManager.OpenScene(@"Assets\Project Data\Game\Scenes\Game.unity");
        }

        [MenuItem("Actions/Game Scene", true)]
        private static bool GameSceneValidation()
        {
            return !Application.isPlaying;
        }

        #endregion

        #region Other

        //[MenuItem("Actions/Print Shorcuts", priority = 150)]
        //private static void PrintShortcuts()
        //{
        //    Debug.Log("H - heal player \nD - toggle player damage \nN - skip level\nR - skip room\n\n");
        //}

        #endregion

#endif
    }
}