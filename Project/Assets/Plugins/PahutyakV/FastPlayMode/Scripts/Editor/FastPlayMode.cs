using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace PahutyakV.FastPlayMode
{
    [Overlay(typeof(SceneView), "Fast Play Mode", true), Icon("d_Refresh")]
    public class FastPlayMode : Overlay
    {
        public override VisualElement CreatePanelContent()
        {
            VisualElement root = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    paddingLeft = 2,
                    paddingRight = 2,
                }
            };

            Button domainButton = CreateOptionButton(
                "d_cs Script Icon",
                "Reload Domain on Play",
                EnterPlayModeOptions.DisableDomainReload
            );
            root.Add(domainButton);

            Button sceneButton = CreateOptionButton(
                "d_Scene",
                "Reload Scene on Play",
                EnterPlayModeOptions.DisableSceneReload
            );
            root.Add(sceneButton);

            return root;
        }

        private Button CreateOptionButton(string iconName, string tooltip, EnterPlayModeOptions option)
        {
            var button = new Button();
            var icon = new Image
            {
                image = EditorGUIUtility.IconContent(iconName).image,
                scaleMode = ScaleMode.ScaleToFit
            };
            button.Add(icon);

            button.clicked += () =>
            {
                var options = EditorSettings.enterPlayModeOptions;
                bool enabled = !options.HasFlag(option);

                if (enabled) options |= option;
                else options &= ~option;

                EditorSettings.enterPlayModeOptions = options;
                EditorSettings.enterPlayModeOptionsEnabled = options != EnterPlayModeOptions.None;

                UpdateButtonStyle(button, !options.HasFlag(option));
                button.tooltip = GetPlayModeOptionsTooltip();
            };

            UpdateButtonStyle(button, !EditorSettings.enterPlayModeOptions.HasFlag(option));
            button.tooltip = GetPlayModeOptionsTooltip();

            button.RegisterCallback<MouseEnterEvent>(evt =>
            {
                button.style.backgroundColor = new Color(0.35f, 0.6f, 1f);
                button.tooltip = GetPlayModeOptionsTooltip();
            });
            button.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                UpdateButtonStyle(button, !EditorSettings.enterPlayModeOptions.HasFlag(option));
            });

            button.style.width = 24;
            button.style.height = 24;
            button.style.unityTextAlign = TextAnchor.MiddleCenter;

            return button;
        }

        private void UpdateButtonStyle(Button button, bool active)
        {
            button.style.backgroundColor = active
                ? new Color(0.25f, 0.5f, 1f)
                : Color.gray;

            button.style.color = active ? Color.white : Color.black;
            button.style.borderTopLeftRadius = 3;
            button.style.borderTopRightRadius = 3;
            button.style.borderBottomLeftRadius = 3;
            button.style.borderBottomRightRadius = 3;
            button.style.paddingLeft = 2;
            button.style.paddingRight = 2;
        }

        private static string GetPlayModeOptionsTooltip()
        {
            var options = EditorSettings.enterPlayModeOptions;
            return $"Enter Play Mode Options:\n" +
                   $"- Domain Reload: {(!options.HasFlag(EnterPlayModeOptions.DisableDomainReload) ? "ON" : "OFF")}\n" +
                   $"- Scene Reload: {(!options.HasFlag(EnterPlayModeOptions.DisableSceneReload) ? "ON" : "OFF")}";
        }
    }

    public static class FastPlayContextMenu
    {
        private const string DomainPath = "Tools/PahutyakV/Fast Play Options/Reload Domain On Play";
        private const string ScenePath = "Tools/PahutyakV/Fast Play Options/Reload Scene On Play";
        private const string ResetPath = "Tools/PahutyakV/Fast Play Options/Reset to Default";
        private const string MoreAssetsPath = "Tools/PahutyakV/Fast Play Options/More Assets";
        private const string HelpPath = "Tools/PahutyakV/Fast Play Options/Help";

        [MenuItem(DomainPath)]
        private static void ToggleDomainReload()
        {
            var options = EditorSettings.enterPlayModeOptions;
            options ^= EnterPlayModeOptions.DisableDomainReload;
            EditorSettings.enterPlayModeOptions = options;
            EditorSettings.enterPlayModeOptionsEnabled = options != EnterPlayModeOptions.None;

            Menu.SetChecked(DomainPath, !options.HasFlag(EnterPlayModeOptions.DisableDomainReload));
        }

        [MenuItem(ScenePath)]
        private static void ToggleSceneReload()
        {
            var options = EditorSettings.enterPlayModeOptions;
            options ^= EnterPlayModeOptions.DisableSceneReload;
            EditorSettings.enterPlayModeOptions = options;
            EditorSettings.enterPlayModeOptionsEnabled = options != EnterPlayModeOptions.None;

            Menu.SetChecked(ScenePath, !options.HasFlag(EnterPlayModeOptions.DisableSceneReload));
        }

        [MenuItem(DomainPath, true)]
        private static bool DomainValidate()
        {
            Menu.SetChecked(DomainPath, !EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload));
            return true;
        }

        [MenuItem(ScenePath, true)]
        private static bool SceneValidate()
        {
            Menu.SetChecked(ScenePath, !EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableSceneReload));
            return true;
        }

        [MenuItem(ResetPath)]
        private static void ResetToDefault()
        {
            EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.None;
            EditorSettings.enterPlayModeOptionsEnabled = false;
            Debug.Log("[Fast Play Mode] Reset to Default (Domain & Scene Reload = ON)");
        }

        [MenuItem(MoreAssetsPath)]
        private static void MoreAssets() => Application.OpenURL("https://assetstore.unity.com/publishers/123098");

        [MenuItem(HelpPath)]
        private static void ShowHelp()
        {
            string msg =
                "Fast Play Mode:\n\n" +
                "- Domain Reload: reloading scripts and statics.\n" +
                "- Scene Reload: reload active scene.\n\n" +
                "Disable these options = Unity enters Play Mode much faster.\n" +
                "But!!! Static variables are not reset, the scene may be in an unusual state.\n\n" +
                "Tip:Use 'Reset to Default' before release or testing.";
            EditorUtility.DisplayDialog("Fast Play Mode - Help", msg, "OK");
        }
    }
}
