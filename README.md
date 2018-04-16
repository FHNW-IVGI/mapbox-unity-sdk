# Mapbox-unity-sdk with GeoServer Support

This fork of [mapbox-unity-sdk](https://github.com/mapbox/mapbox-unity-sdk) allows to retrieve map data from [Mapbox web wervices](https://www.mapbox.com/api-documentation/) and [GeoServer](http://docs.geoserver.org/latest/en/user/index.html). 
GeoServer support is available only in branch [geoserver](https://github.com/FHNW-IVGI/mapbox-unity-sdk/tree/geoserver).

Currently only **vector data** (VectorTiles) is supported.

Modifications are based on [mapbox-unity-sdk_v1.3.0](https://github.com/mapbox/mapbox-unity-sdk/releases/tag/v1.3.0) and tested with **Unity 2017.1.0** and **GeoServer 2.12.1**.

# Getting started

## Building a Unity Package from this Repository


```
git clone https://github.com/FHNW-IVGI/mapbox-unity-sdk.git
cd mapbox-unity-sdk
git checkout geoserver
```

Windows: `update-mapbox-unity-sdk-core.bat`

Linux/Mac: `./update-mapbox-unity-sdk-core.sh`


### Dependencies
This project includes git submodule dependencies outlined in [this gitmodules](https://github.com/mapbox/mapbox-unity-sdk/blob/develop/.gitmodules) file.

To install/update the dependencies after `git clone`/`git pull` run `update-mapbox-unity-sdk-core.bat` or `update-mapbox-unity-sdk-core.sh` (depending on your OS).


### Building a Unity Package
To build a Unity Package for import into your own project from the included `sdkproject`:

1. Start Unity, choose `Open` and navigate to `mapbox-unity-sdk/sdkproject` folder.
2. Select `Mapbox` folder in the project view.
3. Right-click and choose `Export Package...`.

![screen shot 2017-05-26 at 1 14 01 pm](https://cloud.githubusercontent.com/assets/23202691/26509552/7b536a6c-4216-11e7-9f50-b4b461fa73b8.png)

4. Uncheck `Include Dependencies`.

![screen shot 2017-05-26 at 1 14 55 pm](https://cloud.githubusercontent.com/assets/23202691/26509585/9d9677c2-4216-11e7-82ae-c34d150d6d5c.png)

5. Click `Export` and choose a location.

### GeoServer Support
To retrive data from GeoGerver:
1. Configure GeoServer URL in `Mapbox->Setup GeoServer`.
!['Setup GeoServer' screenshot](documentation/docs/images/configure-geoserver-url.png)
2. TODO

# GeoServer Configuration