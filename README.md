# HololensCameraTest

Hololens2のRGBカメラを使うテストプロジェクト  
下記ライブラリを使用する  
1. MRTK2.2
2. OpenCVForUnity 2.4.5(有料プラグインの為、本リポジトリには含めていない)
3. HoloLensWithOpenCVForUnityExample
4. HoloLensCameraStream  


HoloLensCameraStreamは
[VulcanTechnologies/HoloLensCameraStream](https://github.com/VulcanTechnologies/HoloLensCameraStream)   
からフォークした  
[camnewnham/HoloLensCameraStream](https://github.com/camnewnham/HoloLensCameraStream)  
のコードを修正し、UWPで再ビルドして使う。
修正内容は https://github.com/EnoxSoftware/HoloLensWithOpenCVForUnityExample/issues/29 の最下部を参照のこと

> Tested on HoloLens x86 and HoloLens2 ARM64  
> Master/Release
> 
> change the following line in camnewnham / HoloLensCameraStream   
> 
> VideoCapture.cs:  
> static readonly MediaStreamType STREAM_TYPE = MediaStreamType.VideoRecord;   
> to   
> static readonly MediaStreamType STREAM_TYPE = MediaStreamType.VideoPreview;  
> 
> and collect the solution.   
> Replace the resulting HoloLensCameraStream.dll in the Unity Assets \ CamStream \ Plugins \ WSA project
> 
> change the line, in the Unity project CameraIntrinsicsCheckerHelper.cs,  
> static readonly MediaStreamType STREAM_TYPE = MediaStreamType.VideoRecord;  
> to static  
> readonly MediaStreamType STREAM_TYPE = MediaStreamType.VideoPreview  
> 
>Thank you very much @cookieofcode and @Abdul-Mukit

本リポジトリに含まれるHoloLensCameraStream.dllは、上記修正を行いビルドしたもの。

カメラテストの為にカメラアクセスの最低限のコードを記述したシーン"HololensCamTestScene"を作成した。  


camnewnham/HoloLensCameraStreamをARM64でビルドする方法：
2つあるプロジェクト（Dummy , Plugin)のうち、Pluginの最小バージョンを10.0.16229以降に変更する。
設定はプロジェクトのプロパティから行う。
上記を行うと、構成マネージャーからARM64が設定できる。

参考URL:
http://akihiro-document.azurewebsites.net/post/uwp_arm64build/#%E3%83%91%E3%83%83%E3%82%B1%E3%83%BC%E3%82%B8%E6%96%B9%E6%B3%95


