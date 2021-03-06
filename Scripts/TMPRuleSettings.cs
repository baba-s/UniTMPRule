﻿using System;
using System.Linq;
using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// TextMesh Pro の設定のルールをすべて管理するアセット
	/// </summary>
	public sealed class TMPRuleSettings : ScriptableObject
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private TMPRuleSetting[] m_list = new TMPRuleSetting[0];

		//==============================================================================
		// 変数(static)
		//==============================================================================
		private static TMPRuleSettings m_instance;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public TMPRuleSetting[] List => m_list;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 有効になった時に呼び出されます
		/// </summary>
		private void OnEnable()
		{
			m_instance = this;
		}

		//==============================================================================
		// 関数(static)
		//==============================================================================
		/// <summary>
		/// 指定された名前に紐づく設定を返します
		/// </summary>
		public static TMPRuleSetting Find( string name )
		{
			return Array.Find( m_instance.m_list, x => x.Name == name );
		}

#if UNITY_EDITOR
		/// <summary>
		/// ゲームを開始する時に呼び出されます
		/// </summary>
		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
		private static void Initialize()
		{
			if ( m_instance == null ) return;

			var guid = UnityEditor.AssetDatabase
					.FindAssets( $"t:{nameof( TMPRuleSettings )}" )
					.First()
				;

			var path = UnityEditor.AssetDatabase.GUIDToAssetPath( guid );

			m_instance = UnityEditor.AssetDatabase.LoadAssetAtPath<TMPRuleSettings>( path );
		}
#endif
	}
}