using System.IO;
using UnityEditor;
using Mapbox.Unity;
using Mapbox.Unity.Utilities;
using UnityEngine;

namespace Mapbox.Editor
{
    public class GeoServerConfigurationWindow : EditorWindow
    {
        public static GeoServerConfigurationWindow instance;
        static GeoServerAccess.GeoServerConfiguration _config;
        static GeoServerAccess _geoServerAccess;

        static bool _waitingToLoad = false;
	    
	    // default config
	    static string _configurationFile;
	    static string _geoServerUrl = "";
	    
	    //styles
	    GUIStyle _titleStyle;
	    GUIStyle _bodyStyle;
	    GUIStyle _linkStyle;
	    GUIStyle _textFieldStyle;
	    GUIStyle _submitButtonStyle;
	    GUIStyle _verticalGroup;
	    GUIStyle _horizontalGroup;

        [MenuItem("Mapbox/Setup GeoServer")]
        static void InitWhenLoaded()
        {
            if (EditorApplication.isCompiling && !_waitingToLoad)
            {
                //subscribe to updates
                _waitingToLoad = true;
                EditorApplication.update += InitWhenLoaded;
                return;
            }

            if (!EditorApplication.isCompiling)
            {
                //unsubscribe from updates if waiting
                if (_waitingToLoad)
                {
                    EditorApplication.update -= InitWhenLoaded;
                    _waitingToLoad = false;
                }

                Init();
            }
        }

        static void Init()
        {
				Runnable.EnableRunnableInEditor();

				//verify that the config file exists
				_configurationFile = Path.Combine(Constants.Path.MAPBOX_RESOURCES_ABSOLUTE,
					Constants.Path.GEOSERVER_CONFIG_FILE);
				if (!Directory.Exists(Constants.Path.MAPBOX_RESOURCES_ABSOLUTE))
				{
					Directory.CreateDirectory(Constants.Path.MAPBOX_RESOURCES_ABSOLUTE);
				}

				if (!File.Exists(_configurationFile))
				{
					_config = new GeoServerAccess.GeoServerConfiguration
					{
						Url = _geoServerUrl
					};
					var json = JsonUtility.ToJson(_config);
					File.WriteAllText(_configurationFile, json);
					AssetDatabase.Refresh();
				}

				//finish opening the window after the assetdatabase is refreshed.
				EditorApplication.delayCall += OpenWindow;
        }
	    
	    static void OpenWindow()
	    {
		    EditorApplication.delayCall -= OpenWindow;

		    _geoServerAccess = GeoServerAccess.Instance;

		    //setup local variables from GeoServer config file
		    _config = _geoServerAccess.Configuration;
		    if (_config != null)
		    {
			    _geoServerUrl = _config.Url;
		    }

		    //instantiate the config window
		    instance = GetWindow(typeof(GeoServerConfigurationWindow)) as GeoServerConfigurationWindow;
		    instance.titleContent = new GUIContent("GeoServer Setup");
		    instance.Show();
	    }
	    
	    /// <summary>
	    /// Unity Events
	    /// </summary>

	    private void OnDisable() { AssetDatabase.Refresh(); }

	    private void OnDestroy() { AssetDatabase.Refresh(); }

	    private void OnLostFocus() { AssetDatabase.Refresh(); }
	    
	    void OnGUI()
	    {
		    //only run after init
		    if (instance == null)
		    {
			    InitWhenLoaded();
			    return;
		    }

		    InitStyles();

		    // Configuration
		    DrawConfigurationSettings();
	    }
	    
	    void InitStyles()
		{
			_titleStyle = new GUIStyle(GUI.skin.FindStyle("IN TitleText"));
			_titleStyle.padding.left = 3;
			//_titleStyle.fontSize = 16;
			_bodyStyle = new GUIStyle(GUI.skin.FindStyle("WordWrapLabel"));
			//_bodyStyle.fontSize = 14;
			_linkStyle = new GUIStyle(GUI.skin.FindStyle("PR PrefabLabel"));
			//_linkStyle.fontSize = 14;
			_linkStyle.padding.left = 0;
			_linkStyle.padding.top = -1;

			_textFieldStyle = new GUIStyle(GUI.skin.FindStyle("TextField"));
			_textFieldStyle.margin.right = 0;
			_textFieldStyle.margin.top = 0;

			_submitButtonStyle = new GUIStyle(GUI.skin.FindStyle("ButtonRight"));
			_submitButtonStyle.padding.top = 0;
			_submitButtonStyle.margin.top = 0;
			_submitButtonStyle.fixedWidth = 200;

			_verticalGroup = new GUIStyle();
			_verticalGroup.margin = new RectOffset(0, 0, 0, 35);
			_horizontalGroup = new GUIStyle();
			_horizontalGroup.padding = new RectOffset(0, 0, 4, 4);
		}
	    
	    void DrawConfigurationSettings()
	    {
		    EditorGUILayout.LabelField("GeoServer URL", _titleStyle);
		    
		    EditorGUILayout.BeginHorizontal(_horizontalGroup);
		    GUIContent labelContent = new GUIContent("Web admin interface URL, for example:");
		    GUIContent linkContent = new GUIContent("http://my-domain.com/geoserver/");
		    EditorGUILayout.LabelField(labelContent, _bodyStyle, GUILayout.Width(_bodyStyle.CalcSize(labelContent).x));
		    EditorGUILayout.LabelField(linkContent, _linkStyle);
		    GUILayout.FlexibleSpace();
		    EditorGUILayout.EndHorizontal();
		    
		    EditorGUILayout.BeginHorizontal(_horizontalGroup);
		    _geoServerUrl = EditorGUILayout.TextField("", _geoServerUrl, _textFieldStyle);
		    GUILayout.Space(20);
			if (GUILayout.Button("Save", _submitButtonStyle))
			{
				SubmitConfiguration();
			}
			EditorGUILayout.EndHorizontal();
	    }

	    private static void SubmitConfiguration()
	    {
		    var config = new GeoServerAccess.GeoServerConfiguration
		    {
			    Url = _geoServerUrl
		    };
		    _geoServerAccess.SetConfiguration(config);
	    }
    }
}