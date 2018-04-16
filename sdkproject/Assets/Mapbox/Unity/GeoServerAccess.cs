using System.IO;
using UnityEngine;

namespace Mapbox.Unity
{
    public class GeoServerAccess
    {
        static GeoServerAccess _instance;

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static GeoServerAccess Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GeoServerAccess();
                }

                return _instance;
            }
        }

        GeoServerConfiguration _configuration;

        public GeoServerConfiguration Configuration
        {
            get { return _configuration; }
        }

        GeoServerAccess()
        {
            LoadConfiguration();
        }

        /// <summary>
        /// Loads the configuration from Resources folder.
        /// </summary>
        private void LoadConfiguration()
        {
            TextAsset configText = Resources.Load<TextAsset>(Constants.Path.GEOSERVER_RESOURCES_RELATIVE);
#if !WINDOWS_UWP
            SetConfiguration(configText == null ? null : JsonUtility.FromJson<GeoServerConfiguration>(configText.text));
#else
			SetConfiguration(configText == null ? null : Json.JsonConvert.DeserializeObject<GeoServerConfiguration>(configText.text));
#endif
        }

        public void SetConfiguration(GeoServerConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration.Url))
            {
                Debug.LogWarning("GeoServer URL is emplty");
            }
            else if (!configuration.Url.EndsWith("/"))
            {
                // TODO geoAR: check if url is valid GeoServer url
                configuration.Url += "/";
            }

            _configuration = configuration;

            //save the config
            var configFile = Path.Combine(Constants.Path.MAPBOX_RESOURCES_ABSOLUTE, Constants.Path.GEOSERVER_CONFIG_FILE);
            var json = JsonUtility.ToJson(_configuration);
            File.WriteAllText(configFile, json);
        }

        public class GeoServerConfiguration
        {
            public string Url;
        }
    }
}