using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kogane.Internal
{
	/// <summary>
	/// シーン保存時などに TMPRule の設定を反映するエディタ拡張
	/// </summary>
	[InitializeOnLoad]
	internal static class TMPRuleSaver
	{
		//================================================================================
		// 変数(static)
		//================================================================================
		private static TMPRuleSettings m_settings;

		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// スクリプトがコンパイルされた時に呼び出されます
		/// </summary>
		static TMPRuleSaver()
		{
			EditorSceneManager.sceneSaving += ( scene, path ) => OnSceneSaved( scene, path );
		}

		/// <summary>
		/// シーンファイルを開いた時に TMPRule の設定を反映します
		/// </summary>
		private static void OnSceneSaved( Scene scene, string path )
		{
			if ( EditorApplication.isPlaying ) return;
			ApplyAll();
		}

		/// <summary>
		/// シーンに存在するすべての TMPRule の設定を反映します
		/// </summary>
		private static void ApplyAll()
		{
			if ( EditorApplication.isPlaying ) return;

			var tmpRules = GetAllTMPRule();

			foreach ( var tmpRule in tmpRules )
			{
				Apply( tmpRule );
			}
		}

		/// <summary>
		/// 指定された TMPRule の設定を反映します
		/// </summary>
		private static void Apply( TMPRule tmpRule )
		{
			if ( m_settings == null )
			{
				m_settings = TMPRuleEditorUtils.GetSettings();

				if ( m_settings == null ) return;
			}

			TMPRuleEditorUtils.Apply( m_settings, tmpRule );
		}

		/// <summary>
		/// シーンに存在するすべての TMPRule を返します
		/// </summary>
		private static IList<TMPRule> GetAllTMPRule()
		{
			return Resources
					.FindObjectsOfTypeAll<TMPRule>()
					.Where( x => x.gameObject.scene.isLoaded )
					.Where( x => x.gameObject.hideFlags == HideFlags.None )
					.Where( x => PrefabUtility.GetPrefabAssetType( x.gameObject ) == PrefabAssetType.NotAPrefab )
					.ToArray()
				;
		}
	}
}