using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolManagerSO))]
public class CustomPoolManagerSO : Editor
{
	private PoolManagerSO _manager;

	private void OnEnable()
	{
		_manager = target as PoolManagerSO;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if(GUILayout.Button("Get All Pool Item"))
		{
			UpdatePoolingItems();
		}
	}

	private void UpdatePoolingItems()
	{
		List<PoolingItemSO> loadedItems = new List<PoolingItemSO>();

		string[] assetGUIDS = AssetDatabase.FindAssets("", new[]{"Assets/08SO/Pool/Items"});
		
		foreach(string guid in assetGUIDS)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(guid);
			PoolingItemSO item = AssetDatabase.LoadAssetAtPath<PoolingItemSO>(assetPath);

			if(item != null)
			{
				loadedItems.Add(item);
			}
		}

		_manager.poolingItems = loadedItems;

		EditorUtility.SetDirty(_manager);
		AssetDatabase.SaveAssets();
	}
}
