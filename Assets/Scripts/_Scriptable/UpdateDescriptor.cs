using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Update", menuName = "UpdateDescriptor", order = 1)]
public class UpdateDescriptor : ScriptableObject {
    [SerializeField] private UpdateData[] updateData;

    public int UpdatesCount => updateData.Length;

    public UpdateData GetUpdateData(int updateIndex) {
        return updateData[updateIndex];
    }
}

[Serializable]
public class UpdateData {
    public float updateValue;
    public float updatePrice;
}