using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
	/// <summary>
	/// TMPRule のエディタに関する汎用機能を管理するクラス
	/// </summary>
	internal static class TMPRuleEditorUtils
	{
		//==============================================================================
		// 定数
		//==============================================================================
		private const string MENU_ITEM_NAME = "Edit/UniTMPRule/設定ファイルを作成";

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// Unity プロジェクトに存在する TMPRuleSettings を返します
		/// </summary>
		public static TMPRuleSettings GetSettings()
		{
			var guids = AssetDatabase.FindAssets( $"t:{nameof( TMPRuleSettings )}" );
			var path  = AssetDatabase.GUIDToAssetPath( guids.FirstOrDefault() );
			var asset = AssetDatabase.LoadAssetAtPath<TMPRuleSettings>( path );

			return asset;
		}

		/// <summary>
		/// 現在のシーンのすべての TMPRule の設定を反映します
		/// </summary>
		public static void ApplyAllInScene()
		{
			var list = Resources
					.FindObjectsOfTypeAll<GameObject>()
					.Where( x => x.scene.isLoaded )
					.Where( x => x.hideFlags == HideFlags.None )
					.Where( x => !IsPrefabInstance( x ) )
					.Select( x => x.GetComponent<TMPRule>() )
					.Where( x => x != null )
				;

			var settings = GetSettings();

			foreach ( var n in list )
			{
				Apply( settings, n );
			}
		}

		/// <summary>
		/// 指定された TMPRule を持つオブジェクトに設定を反映します
		/// </summary>
		public static void Apply( TMPRuleSettings settings, TMPRule tmpRule )
		{
			var ruleName = tmpRule.RuleName;

			if ( ruleName == TMPRule.INVALID_RULE_NAME ) return;

			var setting = Array.Find( settings.List, c => c.Name == ruleName );

			if ( setting == null ) return;

			var tmpText    = tmpRule.GetComponent<TMP_Text>();
			var canApplyTo = setting.CanApplyTo( tmpText );

			if ( !canApplyTo ) return;

			Undo.RecordObject( tmpText, "Inspector" );
			setting.ApplyTo( tmpText );
			EditorUtility.SetDirty( tmpText );
		}

		/// <summary>
		/// プレハブのインスタンスの場合 true を返します
		/// </summary>
		private static bool IsPrefabInstance( GameObject gameObject )
		{
			return PrefabUtility.GetCorrespondingObjectFromSource( gameObject ) != null &&
			       PrefabUtility.GetPrefabInstanceHandle( gameObject ) != null;
		}

		/// <summary>
		/// TMPRuleSettings.asset を作成するメニュー
		/// </summary>
		[MenuItem( MENU_ITEM_NAME )]
		private static void CreateSettings()
		{
			var settings = ScriptableObject.CreateInstance<TMPRuleSettings>();

			var path = EditorUtility.SaveFilePanel
			(
				title: $"Create {nameof( TMPRuleSettings )}",
				directory: Application.dataPath,
				defaultName: $"{nameof( TMPRuleSettings )}.asset",
				extension: ""
			);

			if ( string.IsNullOrWhiteSpace( path ) ) return;

			path = path.Replace( Application.dataPath, "Assets/" );
			AssetDatabase.CreateAsset( settings, path );
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// TMPRuleSettings.asset を作成するメニューを実行できる場合 true を返します
		/// </summary>
		[MenuItem( MENU_ITEM_NAME, true )]
		private static bool CanCreateSettings()
		{
			return GetSettings() == null;
		}
	}
}