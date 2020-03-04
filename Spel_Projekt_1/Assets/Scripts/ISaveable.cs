using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable {
	byte[] Save();
	void Load(byte[] data, int version);
}
