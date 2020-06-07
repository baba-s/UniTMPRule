using System;
using TMPro;
using UnityEngine;

#pragma warning  disable 0414

namespace Kogane
{
	/// <summary>
	/// TextMesh Pro の設定の個別のルールを管理するクラス
	/// </summary>
	[Serializable]
	public sealed class TMPRuleSetting
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private bool          m_isLock          = default;
		[SerializeField] private string        m_name            = default;
		[SerializeField] private string        m_comment         = default;
		[SerializeField] private TMP_FontAsset m_fontAsset       = default;
		[SerializeField] private Material      m_material        = default;
		[SerializeField] private FontStyles    m_fontStyles      = default;
		[SerializeField] private bool          m_isApplyFontSize = default;
		[SerializeField] private int           m_fontSize        = default;
		[SerializeField] private Color         m_color           = Color.white;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public string   Name     => m_name;
		public string   Comment  => string.IsNullOrWhiteSpace( m_comment ) ? m_name : m_comment;
		public Material Material => m_material;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 指定された TMP_Text に設定を適用できる場合 true を返します
		/// </summary>
		public bool CanApplyTo( TMP_Text tmpText )
		{
			if ( tmpText.font != m_fontAsset ) return true;
			if ( tmpText.fontSharedMaterial != m_material ) return true;
			if ( tmpText.fontStyle != m_fontStyles ) return true;
			if ( tmpText.color != m_color ) return true;
			if ( m_isApplyFontSize && 0.001f < Math.Abs( tmpText.fontSize - m_fontSize ) ) return true;
			return false;
		}

		/// <summary>
		/// 指定された TMP_Text に設定を適用します
		/// </summary>
		public void ApplyTo( TMP_Text tmpText )
		{
			tmpText.font         = m_fontAsset;
			tmpText.fontMaterial = m_material;
			tmpText.fontStyle    = m_fontStyles;
			tmpText.color        = m_color;

			if ( m_isApplyFontSize )
			{
				tmpText.fontSize = m_fontSize;
			}
		}
	}
}