# HueControlWithUnity
## Control your Philips Hue with a Unity App

This project objective is to help you control Philips hue lights through your unity application.

Through a scene example I will show you how to

 * locate your bridge in a local network
 * create a user
 * control lights (On, Off, Fade in)
 * set light colors
 * set the color in real time with a color wheel



 Warning : locate will freeze your ui as currently the code has a thread sleep. I plan on changing that as soon as possible.

## Change log
### Changes in 15 march 2024
- added the color wheel

### Changes in 26 february 2024
- last opened on 2022.3.20F1 LTS 
- changed some obsolete code on the unitywebrequest

### Changes in 11 February 2024
I tested with 2022.3.19F1 LTS as requested and it works.
* WARNING : Unity decided that now http requests are prevented by default. You need to go in player settings and change allow downloads over HTTP* to **always**. That makes the requests insecure, but Hue works in http unfortunately.
* Fun fact : I could make that application work, while I could not get the official application to detect my bridge (It was not used for several years so I hope newer bridge works on the same set of API)


 ## Next functionalities
 
 * get the list of your current lights and states




Licence
Using the free package Clean Vector Icons from PONETI