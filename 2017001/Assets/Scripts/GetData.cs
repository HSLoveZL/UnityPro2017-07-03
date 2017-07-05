using UnityEngine;
using System.IO;
using UnityEngine.VR;
using System;
using System.Text;

public class GetData : MonoBehaviour {

	private Vector3 hmdTrackedObjectedPos;
	private Quaternion hmdTrackedRotation;

	// Use this for initialization
	void Start () {
		FileStream f = new FileStream(@"D:\UnityConsole\test.csv",
			FileMode.OpenOrCreate, FileAccess.Write);
		StreamWriter sw = new StreamWriter(f);
		sw.BaseStream.Seek(0, SeekOrigin.End);
		sw.WriteLine(Environment.NewLine);

		byte[] inputTime = Encoding.UTF8.GetBytes(
			DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss\r\n"));
		f.Position = f.Length;//在文本的末尾追加字符
		f.Write(inputTime, 0, inputTime.Length);
		f.Close();

		InvokeRepeating("GetDatas", 0.0f, 2.0f);
	}
	
	// Update is called once per frame
	void GetDatas () {
		//每次刷新前重置初始位置和旋转角度
		InputTracking.Recenter();
		hmdTrackedObjectedPos = InputTracking.GetLocalPosition(VRNode.CenterEye);
		hmdTrackedRotation = InputTracking.GetLocalRotation(VRNode.CenterEye);

		//Here, we use using to release the memory automaticly
		using (FileStream fileW = new FileStream(@"D:\UnityConsole\test.csv", FileMode.OpenOrCreate, FileAccess.Write))
		{
			StreamWriter sw = new StreamWriter(fileW);
			sw.BaseStream.Seek(0, SeekOrigin.End);
			sw.WriteLine(Environment.NewLine);

			string xyzPos = string.Format("{0:f6},{1:f6},{2:f6},", 
				hmdTrackedObjectedPos.x, hmdTrackedObjectedPos.y, hmdTrackedObjectedPos.z);
			string rotObj = string.Format("{0:f6},{1:f6},{2:f6},{3:f6},{4}\r\n", 
				hmdTrackedRotation.x, hmdTrackedRotation.y, hmdTrackedRotation.z, hmdTrackedRotation.w, DateTime.Now.ToString("HH:mm:ss"));
			byte[] data = Encoding.UTF8.GetBytes(xyzPos + rotObj);
			fileW.Position = fileW.Length;
			fileW.Write(data, 0, data.Length);
		}

		Debug.Log("<color=#50cccc>" + "头盔坐标：" + hmdTrackedObjectedPos.ToString("f6") + "</color>" + "    "
				+ "<color=#3d85c6>" + "头盔方向" + hmdTrackedRotation.ToString("f6") + "</color>" + "\t"
				+ "<color=#a7311a>" + DateTime.Now.ToString("HH:mm:ss") + "</color>");
	}
}
