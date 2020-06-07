using System;
using System.Linq;
using UnityEditor;

namespace Kogane.Internal
{
	/// <summary>
	/// TMPRule の Inspector の表示を変更するエディタ拡張
	/// </summary>
	[CanEditMultipleObjects]
	[CustomEditor( typeof( TMPRule ) )]
	internal sealed class TMPRuleInspector : Editor
	{
		//==============================================================================
		// 変数(static)
		//==============================================================================
		private static TMPRuleSettings m_settings;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// GUI を表示する時に呼び出されます
		/// </summary>
		public override void OnInspectorGUI()
		{
			if ( m_settings == null )
			{
				m_settings = TMPRuleEditorUtils.GetSettings();

				if ( m_settings == null ) return;
			}

			var tmpRuleSelf = ( TMPRule ) target;
			var list        = m_settings.List;
			var index       = Array.FindIndex( list, x => x.Name == tmpRuleSelf.RuleName ) + 1;

			// プルダウンメニューの先頭に「無効」を追加
			var invalidOption = new[] { TMPRule.INVALID_RULE_NAME };
			var options       = invalidOption.Concat( list.Select( x => x.Comment ) ).ToArray();

			EditorGUI.BeginChangeCheck();

			index = EditorGUILayout.Popup( "ルール名", index, options );

			if ( !EditorGUI.EndChangeCheck() ) return;

			var ruleName = index == -1
					? TMPRule.INVALID_RULE_NAME
					: list[ index - 1 ].Name
				;
			
			// 複数選択されている場合に、選択されている
			// すべてのオブジェクトのパラメータを更新するために targets を参照
			foreach ( var tmpRule in targets.OfType<TMPRule>() )
			{
				var serializedObject   = new SerializedObject( tmpRule );
				var serializedProperty = serializedObject.FindProperty( "m_ruleName" );

				serializedProperty.stringValue = ruleName;
				serializedObject.ApplyModifiedProperties();

				TMPRuleEditorUtils.Apply( m_settings, tmpRule );
			}
		}
	}
}