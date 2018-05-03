Query-Chan SD Asset ReadMe

Powered by Pocket Queries, Inc.
http://www.pocket-queries.co.jp
http://www.query-chan.com


-----------------------------------
Asset Information
-----------------------------------
File Name    :  Query-Chan-SD.unitypackage
Version Info.:  Ver. 2.1.0
Release Date :  24th, Feb, 2017

< Version Info >
(13th, Apr, 2015) Ver. 0.0.0 : Pre Release.
(27th, Apr, 2015) Ver. 1.0.0 : Initial Release.
(13th, May, 2015) Ver. 1.1.0 : Add Hokkaido model.
(14th, Feb, 2016) Ver. 1.2.0 : Add 10 local(gotouchi) model.
(11th, Jan, 2017) Ver. 2.0.0 : Add 38 local(gotouchi) model.
(24th, Feb, 2017) Ver. 2.1.0 : Add 2 local(gotouchi) model.(ooita and kumamoto)


-----------------------------------
File Structure
-----------------------------------
Assets
 --> PQAssets
	--> Query-Chan-SD
       --> Animations :  Mecanim animation controller.
	   --> Documents  :  ReadMe file and License logo file.
       --> Materials  :  Material file for demo scene.
       --> Models
          --> SD_QUERY_00  :  Normal Query-chan-SD model (Fat version.)
          --> SD_QUERY_01  :  Normal Query-chan-SD model
          --> SD_QUERY_02  :  Black Query-chan-SD model
          --> SD_QUERY_03  :  Osaka Query-chan-SD model
          --> SD_QUERY_04  :  Fukuoka Query-chan-SD model
          --> SD_QUERY_05  :  Hokkaido Query-chan-SD model
          --> SD_QUERY_06  :  Shimane Query-chan-SD model
          --> SD_QUERY_07  :  Iwate Query-chan-SD model
          --> SD_QUERY_08  :  Fukuoka(New version) Query-chan-SD model
          --> SD_QUERY_09  :  Kyoto Query-chan-SD model
          --> SD_QUERY_10  :  Ibaraki Query-chan-SD model
          --> SD_QUERY_11  :  Shiga Query-chan-SD model
          --> SD_QUERY_12  :  Okayama Query-chan-SD model
          --> SD_QUERY_13  :  Kochi Query-chan-SD model
          --> SD_QUERY_14  :  Mie Query-chan-SD model
              ~~~~~ there are so many models(15 to 53) ~~~~~
          --> SD_QUERY_54  :  SpaceCommand Query-chan-SD model
       --> Prefabs    :  Query-Chan prefabs. (You would use this files for your game scene.)
       --> Props
          --> 01_Osaka   :  Sample 3D model of Naniwa Props.
          --> 02_Fukuoka :  Sample 3D model of Matsuri Props.
          --> 03_Hokkaido:  Sample 3D model of Snow Environment Props
       --> Scenes     :  Sample Scenes.
       --> Scripts
          --> GUIController      :  Scripts for GUI and sample behavior.
          --> QuerySDController  :  Scripts for SD query-chan control.
       --> Sounds     :  BGM (The thema of Query-Chan) and sample voice files.
       --> Textures   :  Textures for demo scene.


-----------------------------------
How to use Query-Chan
-----------------------------------
1. Import Query-Chan-SD.unitypackage to your Game project.

2. Find Query-Chan-SD_xxxxx.prefab in Prefabs folder and Drop it to your Game scne (hierarchy).

3. Press Play button and play game, you can see Query-Chan with default animation (animation name : 0001_Stand).

4. You can control Query-chan by using below scripts.

   <How to control mecanim animations>
     - Use ChangeAnimation() method in the QuerySDMecanimController class.
       You can use 44 animations. (Please see "enum QueryChanSDAnimationType" in the class file.)

   <How to control emotions>
     - Use ChangeEmotion() method in the QuerySDEmotionalController class.
       You can use 7 emotions. (Please see "enum QueryChanSDEmotionalType" in the class file.)

   <How to control Sounds>
     - Use PlaySoundByType() method in the QuerySoundController class.
       You can use 1 sample sounds. (Please see "enum QueryChanSoundType" in the class file.)
       Please see below URL, if you would like to get more query-chan voice files.
         https://www.assetstore.unity3d.com/en/#!/content/20031


-----------------------------------
Demo Scene
-----------------------------------
1. You should add below scene files to "Scenes in build" in the "Build Settings window".
     - 01_OperateQuerySD_Normal.unity
     - 01_OperateQuerySD_Normal_MikanButori.unity
     - 02_OperateQuerySD_Black.unity
     - 03_OperateQuerySD_Osaka.unity
     - 04_OperateQuerySD_Fukuoka.unity
     - 05_OperateQuerySD_Hokkaido.unity
     - 06_OperateQuerySD_Shimane.unity
     - 07_OperateQuerySD_Iwate.unity
     - 08_OperateQuerySD_NewFukuoka.unity
     - 09_OperateQuerySD_Kyoto.unity
     - 10_OperateQuerySD_Ivaraki.unity
     - 11_OperateQuerySD_Shiga.unity
     - 12_OperateQuerySD_Okayama.unity
     - 13_OperateQuerySD_Kochi.unity
     - 14_OperateQuerySD_Mie.unity
      ~~~~~ and Add All scene files in this folder ~~~~~
     - 99_QuerySD_ShowTime.unity

2. Play game, you can change animations, emotions and also can play voice sounds to press buttons on views.

3. Scene file -99_QuerySD_ShowTime.unity- is the present from query-chan. enjoy it!


-----------------------------------
Logo Icon File
-----------------------------------
You are free to use "Query-Chan Logo Image File" below.
We are glad that you use the logo image on your Game applications.

   Assets/PQAssets/Query-Chan-SD/Documents/Query-Chan_license_logo.png
