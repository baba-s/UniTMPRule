using System;
using System.Linq;
using Kogane.Internal;
using TMPro;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// TMPRuleSettings の Inspector の表示を変更するエディタ拡張
	/// </summary>
	[CustomEditor( typeof( TMPRuleSettings ) )]
	public sealed class TMPRuleSettingsInspector : Editor
	{
		//==============================================================================
		// 変数
		//==============================================================================
		private SerializedProperty m_property;
		private ReorderableList    m_reorderableList;

		//==============================================================================
		// デリゲート(static)
		//==============================================================================
		public new static Action<TMPRuleSettings> OnHeaderGUI { get; set; }
		public static     Action<TMPRuleSettings> OnFooterGUI { get; set; }

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 有効になった時に呼び出されます
		/// </summary>
		private void OnEnable()
		{
			TMPRuleSettingDrawer.FontAssetList = FindAllFontAsset();
			TMPRuleSettingDrawer.MaterialList  = FindAllMaterial();

			m_property = serializedObject.FindProperty( "m_list" );

			m_reorderableList = new ReorderableList( serializedObject, m_property )
			{
				drawElementCallback   = OnDrawElement,
				elementHeightCallback = OnElementHeight,
			};
		}

		/// <summary>
		/// 無効になった時に呼び出されます
		/// </summary>
		private void OnDisable()
		{
			TMPRuleSettingDrawer.FontAssetList = null;
			TMPRuleSettingDrawer.MaterialList  = null;

			m_property        = null;
			m_reorderableList = null;
		}

		/// <summary>
		/// リストの要素を描画する時に呼び出されます
		/// </summary>
		private void OnDrawElement
		(
			Rect rect,
			int  index,
			bool isActive,
			bool isFocused
		)
		{
			var element = m_property.GetArrayElementAtIndex( index );
			rect.height -= 4;
			rect.y      += 2;
			EditorGUI.PropertyField( rect, element );
		}

		/// <summary>
		/// リストの要素の高さを返します
		/// </summary>
		private float OnElementHeight( int index )
		{
			var element = m_property.GetArrayElementAtIndex( index );
			var isOpen  = element.isExpanded;

			return isOpen ? 216 : 24;
		}

		/// <summary>
		/// GUI を表示する時に呼び出されます
		/// </summary>
		public override void OnInspectorGUI()
		{
			if ( GUILayout.Button( "現在のシーンのすべてのオブジェクトに反映" ) )
			{
				TMPRuleEditorUtils.ApplyAllInScene();
			}

			serializedObject.Update();

			var settings = ( TMPRuleSettings ) target;

			OnHeaderGUI?.Invoke( settings );
			m_reorderableList.DoLayoutList();
			OnFooterGUI?.Invoke( settings );
			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// Unity プロジェクトに存在するすべての FontAsset を検索します
		/// </summary>
		private static TMP_FontAsset[] FindAllFontAsset()
		{
			return AssetDatabase
				.FindAssets( $"t:{nameof( TMP_FontAsset )}" )
				.Select( x => AssetDatabase.GUIDToAssetPath( x ) )
				.Select( x => AssetDatabase.LoadAssetAtPath<TMP_FontAsset>( x ) )
				.ToArray();
		}

		/// <summary>
		/// Unity プロジェクトに存在するすべてのマテリアルを検索します
		/// </summary>
		private static Material[] FindAllMaterial()
		{
			return AssetDatabase
				.FindAssets( $"t:{nameof( Material )}" )
				.Select( x => AssetDatabase.GUIDToAssetPath( x ) )
				.Select( x => AssetDatabase.LoadAssetAtPath<Material>( x ) )
				.Where( x => x != null )
				.ToArray();
		}
	}
}