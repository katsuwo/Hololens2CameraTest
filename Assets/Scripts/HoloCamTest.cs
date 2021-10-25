using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using OpenCVForUnity.ArucoModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnity.Calib3dModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using HoloLensWithOpenCVForUnity.UnityUtils.Helper;
using HoloLensCameraStream;
using Microsoft.MixedReality.Toolkit.Input;

namespace HoloLensWithOpenCVForUnityExample {
	public class HoloCamTest : MonoBehaviour {
		// The webcam texture to mat helper.
		private HololensCameraStreamToMatHelper _webCamTextureToMatHelper;
		readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();

		Mat rgbMat4preview;
		Texture2D _texture;
		[SerializeField] private GameObject previewQuad;

		void Start() {
			Debug.Log("Iam started.");
			_webCamTextureToMatHelper = gameObject.GetComponent<HololensCameraStreamToMatHelper>();
			_webCamTextureToMatHelper.frameMatAcquired += OnFrameMatAcquired;
			_webCamTextureToMatHelper.outputColorFormat = WebCamTextureToMatHelper.ColorFormat.GRAY;
			_webCamTextureToMatHelper.Initialize();
		}

		public void OnWebCamTextureToMatHelperInitialized() {
			Debug.Log("Initialize started.");
			Mat grayMat = _webCamTextureToMatHelper.GetMat();
			float rawFrameWidth = grayMat.width();
			float rawFrameHeight = grayMat.height();

			_texture = new Texture2D((int) grayMat.width(), (int) grayMat.height(), TextureFormat.RGB24, false);
			previewQuad.GetComponent<MeshRenderer>().material.mainTexture = _texture;
			Debug.Log("Initialize done.");
		}

		public void OnWebCamTextureToMatHelperDisposed() {
			Debug.Log("OnWebCamTextureToMatHelperDisposed");
		}

		public void OnWebCamTextureToMatHelperErrorOccurred(WebCamTextureToMatHelper.ErrorCode errorCode) {
			Debug.Log("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
		}

		public void OnFrameMatAcquired(Mat grayMat, Matrix4x4 projectionMatrix, Matrix4x4 cameraToWorldMatrix,
			CameraIntrinsics cameraIntrinsics) {
			Debug.Log("OnFrameMatAcquired ");
			DebugUtils.VideoTick();

			Mat camMatrix = null;
			MatOfDouble distCoeffs = null;

			rgbMat4preview = new Mat();
			Imgproc.cvtColor(grayMat, rgbMat4preview, Imgproc.COLOR_GRAY2RGB);

			DebugUtils.TrackTick();
			Enqueue(() => {
				Debug.Log("QUEUED ACTION INVOKED.");
				if (!_webCamTextureToMatHelper.IsPlaying())
				{
					Debug.Log("WEBCAMTEXTUREHELPRE IS NOT RUNNING.");
					return;
				}

				if (rgbMat4preview != null)
				{
					Utils.fastMatToTexture2D(rgbMat4preview, _texture);
					Debug.Log("RGBMAT => TEXTURE.");
					rgbMat4preview.Dispose();
				}
				grayMat.Dispose();
			});
		}

		private void Enqueue(Action action) {
			lock (ExecuteOnMainThread)
			{
				ExecuteOnMainThread.Enqueue(action);
			}
		}

		private Mat CreateCameraMatrix(double fx, double fy, double cx, double cy) {
			Mat camMatrix = new Mat(3, 3, CvType.CV_64FC1);
			camMatrix.put(0, 0, fx);
			camMatrix.put(0, 1, 0);
			camMatrix.put(0, 2, cx);
			camMatrix.put(1, 0, 0);
			camMatrix.put(1, 1, fy);
			camMatrix.put(1, 2, cy);
			camMatrix.put(2, 0, 0);
			camMatrix.put(2, 1, 0);
			camMatrix.put(2, 2, 1.0f);

			return camMatrix;
		}


		// Update is called once per frame
		void Update() {
			lock (ExecuteOnMainThread)
			{
				while (ExecuteOnMainThread.Count > 0)
				{
					ExecuteOnMainThread.Dequeue().Invoke();
					Debug.Log("INVOKED");
				}
			}

			if (_webCamTextureToMatHelper.IsPlaying() && _webCamTextureToMatHelper.DidUpdateThisFrame())
			{
				DebugUtils.VideoTick();
/*
				if (enableDetection && !isDetecting)
				{
					isDetecting = true;

					Mat grayMat = webCamTextureToMatHelper.GetMat();

					if (enableDownScale)
					{
						downScaleMat = imageOptimizationHelper.GetDownScaleMat(grayMat);
						DOWNSCALE_RATIO = imageOptimizationHelper.downscaleRatio;
					}
					else
					{
						downScaleMat = grayMat;
						DOWNSCALE_RATIO = 1.0f;
					}

					StartThread(ThreadWorker);
				}
*/
			}
		}
	}
}