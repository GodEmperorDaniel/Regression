using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class FungusSaver : ISaveable {
	public class SaveObj {
		public string sceneName = "";
		public string objName = "";
		public List<BoolVar> boolVars = new List<BoolVar>();
		public List<IntVar> intVars = new List<IntVar>();
		public List<FloatVar> floatVars = new List<FloatVar>();
		public List<StringVar> stringVars = new List<StringVar>();
	}

	private static FungusSaver _instance;
	private Dictionary<string, Flowchart> flowcharts = new Dictionary<string, Flowchart>();
	private Dictionary<string, SaveObj> saveData = new Dictionary<string, SaveObj>();

	public static Encoding StringEncoding = Encoding.Unicode;

	public static FungusSaver Instance {
		get {
			if (_instance == null) {
				_instance = new FungusSaver();
			}
			return _instance;
		}
	}

	public void ClearSave() {
		flowcharts = new Dictionary<string, Flowchart>();
		saveData = new Dictionary<string, SaveObj>();
	}

	public void Load(byte[] data, int version) {
		var flowchartCount = BitConverter.ToInt32(data, 0);

		var index = 4;

		var globalSaveObj = Decode(data, ref index);

		for (int i = 0; i < globalSaveObj.boolVars.Count; i++) {
			var boolVar = globalSaveObj.boolVars[i];
			FungusManager.Instance.GlobalVariables.GetOrAddVariable(boolVar.Key, boolVar.Value, typeof(BooleanVariable)).Value = boolVar.Value;
		}
		for (int i = 0; i < globalSaveObj.intVars.Count; i++) {
			var intVar = globalSaveObj.intVars[i];
			FungusManager.Instance.GlobalVariables.GetOrAddVariable(intVar.Key, intVar.Value, typeof(IntegerVariable)).Value = intVar.Value;
		}
		for (int i = 0; i < globalSaveObj.floatVars.Count; i++) {
			var floatVar = globalSaveObj.floatVars[i];
			FungusManager.Instance.GlobalVariables.GetOrAddVariable(floatVar.Key, floatVar.Value, typeof(FloatVariable)).Value = floatVar.Value;
		}
		for (int i = 0; i < globalSaveObj.stringVars.Count; i++) {
			var stringVar = globalSaveObj.stringVars[i];
			FungusManager.Instance.GlobalVariables.GetOrAddVariable(stringVar.Key, stringVar.Value, typeof(StringVariable)).Value = stringVar.Value;
		}

		for (var i = 0; i < flowchartCount; i++) {
			var saveObj = Decode(data, ref index);

			var key = GetKey(saveObj);

			if (saveData.ContainsKey(key)) {
				saveData[key] = saveObj;
			} else {
				saveData.Add(key, saveObj);
			}

			if (flowcharts.ContainsKey(key)) {
				WriteToFlowchart(flowcharts[key], saveObj);
			}
		}
	}

	public byte[] Save() {
		foreach (var flowchart in flowcharts) {
			ReadFromFlowchart(flowchart.Value);
		}

		var objs = new byte[saveData.Count + 1][];

		objs[0] = Encode(FungusManager.Instance.GlobalVariables.StoreVariables());
		var totalSize = 4 + objs[0].Length;

		var i = 1;
		foreach (var pair in saveData) {
			var data = Encode(pair.Value);

			objs[i] = data;

			totalSize += data.Length;
			i++;
		}

		var output = new byte[totalSize];

		BitConverter.GetBytes(saveData.Count).CopyTo(output,0);

		var index = 4;
		for (var j = 0; j < objs.Length; j++) {
			objs[j].CopyTo(output, index);
			index += objs[j].Length;
		}

		return output;
	}

	public void FlowchartEnabled(Flowchart flowchart) {
		var key = GetKey(flowchart);
		flowcharts.Add(key, flowchart);
		if (saveData.ContainsKey(key) && Application.isPlaying) {
			WriteToFlowchart(flowcharts[key], saveData[key]);
		}
	}

	public void FlowchartDisabled(Flowchart flowchart) {
		ReadFromFlowchart(flowchart);
		flowcharts.Remove(GetKey(flowchart));
	}

	private byte[] Encode(SaveObj saveObj) {
		var bools = new byte[1 + saveObj.boolVars.Count][];
		var ints = new byte[1 + saveObj.intVars.Count][];
		var floats = new byte[1 + saveObj.floatVars.Count][];
		var strings = new byte[1 + saveObj.stringVars.Count][];

		var objName = StringEncoding.GetBytes(saveObj.objName);
		var sceneName = StringEncoding.GetBytes(saveObj.sceneName);
		var objL = BitConverter.GetBytes(objName.Length);
		var sceneL = BitConverter.GetBytes(sceneName.Length);
		bools[0] = BitConverter.GetBytes(saveObj.boolVars.Count);
		ints[0] = BitConverter.GetBytes(saveObj.intVars.Count);
		floats[0] = BitConverter.GetBytes(saveObj.floatVars.Count);
		strings[0] = BitConverter.GetBytes(saveObj.stringVars.Count);

		var size = 24 + objName.Length + sceneName.Length;

		for (var i = 0; i < saveObj.boolVars.Count; i++) {
			bools[i + 1] = saveObj.boolVars[i].Save();
			size += bools[i + 1].Length;
		}
		for (var i = 0; i < saveObj.intVars.Count; i++) {
			ints[i + 1] = saveObj.intVars[i].Save();
			size += ints[i + 1].Length;
		}
		for (var i = 0; i < saveObj.floatVars.Count; i++) {
			floats[i + 1] = saveObj.floatVars[i].Save();
			size += floats[i + 1].Length;
		}
		for (var i = 0; i < saveObj.stringVars.Count; i++) {
			strings[i + 1] = saveObj.stringVars[i].Save();
			size += strings[i + 1].Length;
		}

		var outputData = new byte[size];

		objL.CopyTo(outputData, 0);
		sceneL.CopyTo(outputData, 4);
		objName.CopyTo(outputData, 8);
		sceneName.CopyTo(outputData, 8 + objName.Length);

		var index = 8 + objName.Length + sceneName.Length;

		foreach (var t in bools) {
			t.CopyTo(outputData, index);
			index += t.Length;
		}
		foreach (var t in ints) {
			t.CopyTo(outputData, index);
			index += t.Length;
		}
		foreach (var t in floats) {
			t.CopyTo(outputData, index);
			index += t.Length;
		}
		foreach (var t in strings) {
			t.CopyTo(outputData, index);
			index += t.Length;
		}

		return outputData;
	}

	private SaveObj Decode(byte[] data, ref int index) {
		var objectNameLength = BitConverter.ToInt32(data, index);
		index += 4;
		var sceneNameLength = BitConverter.ToInt32(data, index);
		index += 4;
		var objectName = StringEncoding.GetString(data, index, objectNameLength);
		index += objectNameLength;
		var sceneName = StringEncoding.GetString(data, index, sceneNameLength);
		index += sceneNameLength;
		var boolCount = BitConverter.ToInt32(data, index);
		index += 4;

		var boolVars = new List<BoolVar>();
		var intVars = new List<IntVar>();
		var floatVars = new List<FloatVar>();
		var stringVars = new List<StringVar>();

		for (var j = 0; j < boolCount; j++) {
			var v = new BoolVar();

			var keyLength = BitConverter.ToInt32(data, index);
			index += 4;
			var valueLength = BitConverter.ToInt32(data, index);
			index += 4;
			v.Key = StringEncoding.GetString(data, index, keyLength);
			index += keyLength;
			v.Value = BitConverter.ToBoolean(data, index);
			index += valueLength;

			boolVars.Add(v);
		}

		var intCount = BitConverter.ToInt32(data, index);
		index += 4;

		for (var j = 0; j < intCount; j++) {
			var v = new IntVar();

			var keyLength = BitConverter.ToInt32(data, index);
			index += 4;
			var valueLength = BitConverter.ToInt32(data, index);
			index += 4;
			v.Key = StringEncoding.GetString(data, index, keyLength);
			index += keyLength;
			v.Value = BitConverter.ToInt32(data, index);
			index += valueLength;

			intVars.Add(v);
		}

		var floatCount = BitConverter.ToInt32(data, index);
		index += 4;

		for (var j = 0; j < floatCount; j++) {
			var v = new FloatVar();

			var keyLength = BitConverter.ToInt32(data, index);
			index += 4;
			var valueLength = BitConverter.ToInt32(data, index);
			index += 4;
			v.Key = StringEncoding.GetString(data, index, keyLength);
			index += keyLength;
			v.Value = BitConverter.ToSingle(data, index);
			index += valueLength;

			floatVars.Add(v);
		}

		var stringCount = BitConverter.ToInt32(data, index);
		index += 4;

		for (var j = 0; j < stringCount; j++) {
			var v = new StringVar();

			var keyLength = BitConverter.ToInt32(data, index);
			index += 4;
			var valueLength = BitConverter.ToInt32(data, index);
			index += 4;
			v.Key = StringEncoding.GetString(data, index, keyLength);
			index += keyLength;
			v.Value = StringEncoding.GetString(data, index, valueLength);
			index += valueLength;

			stringVars.Add(v);
		}

		var saveObj = new SaveObj();
		saveObj.sceneName = sceneName;
		saveObj.objName = objectName;
		saveObj.floatVars = floatVars;
		saveObj.intVars = intVars;
		saveObj.boolVars = boolVars;
		saveObj.stringVars = stringVars;

		return saveObj;
	}

	private void ReadFromFlowchart(Flowchart flowchart) {
		var key = GetKey(flowchart);
		if (!saveData.ContainsKey(key)) {
			saveData.Add(key, new SaveObj());
		}

		var data = saveData[key];

		data.objName = GetPath(flowchart);
		data.sceneName = flowchart.gameObject.scene.name;
		data.floatVars.Clear();
		data.boolVars.Clear();
		data.stringVars.Clear();
		data.intVars.Clear();

		for (int i = 0; i < flowchart.Variables.Count; i++) {
			var v = flowchart.Variables[i];

			if (v.Scope == VariableScope.Global) {
				continue;
			}

			// Save string
			var stringVariable = v as StringVariable;
			if (stringVariable != null) {
				var d = new StringVar();
				d.Key = stringVariable.Key;
				d.Value = stringVariable.Value;
				data.stringVars.Add(d);
			}

			// Save int
			var intVariable = v as IntegerVariable;
			if (intVariable != null) {
				var d = new IntVar();
				d.Key = intVariable.Key;
				d.Value = intVariable.Value;
				data.intVars.Add(d);
			}

			// Save float
			var floatVariable = v as FloatVariable;
			if (floatVariable != null) {
				var d = new FloatVar();
				d.Key = floatVariable.Key;
				d.Value = floatVariable.Value;
				data.floatVars.Add(d);
			}

			// Save bool
			var boolVariable = v as BooleanVariable;
			if (boolVariable != null) {
				var d = new BoolVar();
				d.Key = boolVariable.Key;
				d.Value = boolVariable.Value;
				data.boolVars.Add(d);
			}
		}
	}

	private void WriteToFlowchart(Flowchart flowchart, SaveObj saveObj) {
		for (int i = 0; i < saveObj.boolVars.Count; i++) {
			var boolVar = saveObj.boolVars[i];
			flowchart.SetBooleanVariable(boolVar.Key, boolVar.Value);
		}
		for (int i = 0; i < saveObj.intVars.Count; i++) {
			var intVar = saveObj.intVars[i];
			flowchart.SetIntegerVariable(intVar.Key, intVar.Value);
		}
		for (int i = 0; i < saveObj.floatVars.Count; i++) {
			var floatVar = saveObj.floatVars[i];
			flowchart.SetFloatVariable(floatVar.Key, floatVar.Value);
		}
		for (int i = 0; i < saveObj.stringVars.Count; i++) {
			var stringVar = saveObj.stringVars[i];
			flowchart.SetStringVariable(stringVar.Key, stringVar.Value);
		}
	}

	private string GetKey(Flowchart flowchart) {
		return flowchart.gameObject.scene.name + " + " + GetPath(flowchart);
	}

	private string GetKey(SaveObj saveObj) {
		return saveObj.sceneName + " + " + saveObj.objName;
	}

	private string GetPath(Flowchart flowchart) {
		var obj = flowchart.gameObject;
		string path = "/" + obj.name;
		while (obj.transform.parent != null) {
			obj = obj.transform.parent.gameObject;
			path = "/" + obj.name + path;
		}
		return path;
	}
}
