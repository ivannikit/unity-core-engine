using UnityEngine;
using UnityEditor;
using System.Linq;

namespace TeamZero.Inspector
{
    [CustomPropertyDrawer(typeof(TypeRistrictionAttribute))]
    public class TypeRistrictionAttibuteDrawer : PropertyDrawer
    {
        private TypeRistrictionAttribute _ristrictionAttibute;
        private TypeRistrictionAttribute RistrictionAttibute
        {
            get
            {
                if (_ristrictionAttibute == null)
                    _ristrictionAttibute = attribute as TypeRistrictionAttribute;
                return _ristrictionAttibute;
            }
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property) { return true; }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0f;
            if (IsSupported(property))
            {
                height += EditorGUIUtility.singleLineHeight;
                if (_showInfo && RistrictionAttibute != null)
                {
                    height += EditorGUIUtility.singleLineHeight * RistrictionAttibute.RistrictionTypes.Length;
                }
            }
            else
            {
                // warning message
                height = 38f;
            }

            return height;
        }

        private bool IsSupported(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                return true;
            }
            else
            {
                System.Type ft = GetFieldType(property);
                var attributes = System.Attribute.GetCustomAttributes(ft, true);
                var target = attributes.FirstOrDefault(a => a is TypeRistrictionTargetAttribute) as TypeRistrictionTargetAttribute;
                return target != null && string.IsNullOrEmpty(target.FieldName) == false;
            }
        }

        private System.Type GetFieldType(SerializedProperty property)
        {
            System.Type parentType = property.serializedObject.targetObject.GetType();
            System.Reflection.FieldInfo fi = parentType.GetField(property.propertyPath);
            return fi.FieldType;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty targetProperty = null;
            System.Type targetType = null;
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                targetProperty = property;
                targetType = GetFieldType(property);
            }
            else
            {
                System.Type ft = GetFieldType(property);
                var attributes = System.Attribute.GetCustomAttributes(ft, true);
                var target = attributes.FirstOrDefault(a => a is TypeRistrictionTargetAttribute) as TypeRistrictionTargetAttribute;
                if (target != null && string.IsNullOrEmpty(target.FieldName) == false)
                {
                    targetProperty = property.FindPropertyRelative(target.FieldName);
                    targetType = typeof(Object);
                }
                else
                {
                    EditorGUI.HelpBox(position, string.Format("TypeRistrictionAttibute not support {0}", ft), MessageType.Warning);
                    return;
                }
            }

            Draw(RistrictionAttibute, position, targetProperty, targetType, label);
        }

        private GUIStyle _titleStyle;
        private GUIStyle TitleStyle
        {
            get
            {
                if (_titleStyle == null)
                {
                    _titleStyle = new GUIStyle(EditorStyles.foldout);
                    _titleStyle.richText = true;
                }

                return _titleStyle;
            }
        }

        private GUIStyle _infoStyle;
        private GUIStyle InfoStyle
        {
            get
            {
                if (_infoStyle == null)
                {
                    _infoStyle = new GUIStyle(EditorStyles.label);
                    _infoStyle.richText = true;
                }

                return _infoStyle;
            }
        }

        private bool IsRistrictionListExist { get { return RistrictionAttibute != null && RistrictionAttibute.RistrictionTypes.Length > 1; } }

        private bool _showInfo = false;
        private const float FOLDOUT_WIDTH = 20f;
        private const string RISTRICTION_COLOR = "#505050ff";
        private void Draw(TypeRistrictionAttribute attribute, Rect position, SerializedProperty property, System.Type propertyObjectType, GUIContent label)
        {
            if (property == null || propertyObjectType == null)
                return; // данный тип сериализуемых полей не поддерживается аттрибутом

            // задаём область шапки
            Rect firstLinePosition = position;
            firstLinePosition.height = EditorGUIUtility.singleLineHeight;

            // добавляем в label информацию об ограничениях
            if (!_showInfo && RistrictionAttibute != null)
            {
                var ristrictions = RistrictionAttibute.RistrictionTypes;
                string message = string.Empty;
                if (ristrictions.Length == 1)
                    message = string.Format("({0})", ristrictions[0].Name);
                else
                    message = string.Format("({0} ristrictions)", ristrictions.Length);

                label.text = string.Format("{0} <color={1}>{2}</color>", label.text, RISTRICTION_COLOR, message);
            }

            // отрисовываем label
            if (IsRistrictionListExist)
            {
                Rect foldoutPosition = firstLinePosition;
                foldoutPosition.width = FOLDOUT_WIDTH;
                _showInfo = EditorGUI.Foldout(foldoutPosition, _showInfo, label, TitleStyle);
            }
            else
            {
                Rect labelPosition = firstLinePosition;
                labelPosition.width = EditorGUIUtility.labelWidth;
                EditorGUI.LabelField(labelPosition, label, InfoStyle);
            }

            EditorGUI.BeginChangeCheck();

            // отрисовываем редактируемое поле
            Rect valuePosition = firstLinePosition;
            valuePosition.x += EditorGUIUtility.labelWidth;
            valuePosition.width -= EditorGUIUtility.labelWidth;
            Object value = property.objectReferenceValue;
            value = EditorGUI.ObjectField(valuePosition, GUIContent.none, property.objectReferenceValue, propertyObjectType, true);

            // показываем информацию об ограничениях типа
            if (IsRistrictionListExist && _showInfo)
            {
                Rect infoPosition = position;
                infoPosition.y += EditorGUIUtility.singleLineHeight;
                infoPosition.height -= EditorGUIUtility.singleLineHeight;
                DrawInfo(infoPosition);
            }

            // изменяем значение, если прошли ограничения
            if (EditorGUI.EndChangeCheck())
            {
                property.objectReferenceValue = CheckTypeRistriction(value, attribute);
            }
        }

        private void DrawInfo(Rect position)
        {
            if (RistrictionAttibute == null)
                return;

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;

            Rect linePosition = position;
            linePosition.height = EditorGUIUtility.singleLineHeight;
            foreach (System.Type ristriction in RistrictionAttibute.RistrictionTypes)
            {
                string lineText = string.Format("<color={1}>{0}</color>", ristriction.Name, RISTRICTION_COLOR);
                EditorGUI.LabelField(linePosition, lineText, InfoStyle);
                linePosition.y += EditorGUIUtility.singleLineHeight;
            }

            EditorGUI.indentLevel = indent;
        }

        private Object CheckTypeRistriction(Object value, TypeRistrictionAttribute attribute)
        {
            if (value == null || attribute == null)
                return null;

            if (value is GameObject)
            {
                var go = value as GameObject;
                var components = go.GetComponents<MonoBehaviour>();
                foreach (var c in components)
                {
                    if (CheckTypeRistrictionImpl(c, attribute))
                        return c;
                }
            }
            else if (CheckTypeRistrictionImpl(value, attribute))
            {
                return value;
            }

            return null;
        }

        private bool CheckTypeRistrictionImpl(Object value, TypeRistrictionAttribute attribute)
        {
            System.Type valueType = value.GetType();
            foreach (System.Type ristriction in attribute.RistrictionTypes)
            {
                if (ristriction.IsAssignableFrom(valueType) == false)
                    return false;
            }

            return true;
        }
    }
}
