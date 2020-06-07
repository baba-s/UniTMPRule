using TMPro;
using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// TextMesh Pro の設定をルールに沿って変更するコンポーネント
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent( typeof( TMP_Text ) )]
	public sealed class TMPRule : MonoBehaviour
	{
		//==============================================================================
		// 定数
		//==============================================================================
		public const string INVALID_RULE_NAME = "無効";

		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private string m_ruleName = INVALID_RULE_NAME;

		//==============================================================================
		// プロパティ(SerializeField)
		//==============================================================================
		public string RuleName => m_ruleName;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// ルールを設定します
		/// </summary>
		public void SetRule( string ruleName )
		{
			var ruleParam = TMPRuleSettings.Find( ruleName );
			var tempText  = GetComponent<TMP_Text>();

			m_ruleName = ruleName;

			ruleParam.ApplyTo( tempText );
		}
	}
}