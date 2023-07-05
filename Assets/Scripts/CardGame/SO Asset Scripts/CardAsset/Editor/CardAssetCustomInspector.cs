using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardAsset)), CanEditMultipleObjects]
public class CardAssetCustomInspector : Editor
{
    public SerializedProperty
    Description_prop,
    CardImage_prop,
    TypeOfCard_prop,
    SubTypeOfCard_prop,
    TypeOfEquipment_prop,
    SpellScriptName_prop,
    SpecialSpellAmount_prop,
    Targets_prop,
    SpellAttribute_prop,
    Suits_prop,
    CardRank_prop,
    WeaponAttackDistance_prop,
        SubTypeOfTip_prop
    ;

    void OnEnable()
    {
        // Setup the SerializedProperties

        // all the common general fields
        Description_prop = serializedObject.FindProperty("Description");
        CardImage_prop = serializedObject.FindProperty("CardImage");
        TypeOfCard_prop = serializedObject.FindProperty("TypeOfCard");
        SubTypeOfCard_prop = serializedObject.FindProperty("SubTypeOfCard");
        TypeOfEquipment_prop = serializedObject.FindProperty("TypeOfEquipment");

        // all the spell fields:
        SpellScriptName_prop = serializedObject.FindProperty("SpellScriptName");
        SpecialSpellAmount_prop = serializedObject.FindProperty("SpecialSpellAmount");
        Targets_prop = serializedObject.FindProperty("Targets");
        SpellAttribute_prop = serializedObject.FindProperty("SpellAttribute");
        CardRank_prop = serializedObject.FindProperty("CardRank");
        Suits_prop = serializedObject.FindProperty("Suits");
        WeaponAttackDistance_prop = serializedObject.FindProperty("WeaponAttackDistance");
        SubTypeOfTip_prop = serializedObject.FindProperty("SubTypeOfTip");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(Description_prop);
        EditorGUILayout.PropertyField(CardImage_prop);
        EditorGUILayout.PropertyField(TypeOfCard_prop);
        EditorGUILayout.PropertyField(SubTypeOfCard_prop);


        TypesOfCards st = (TypesOfCards)TypeOfCard_prop.enumValueIndex;
        TypeOfEquipment typeOfEquipment = (TypeOfEquipment)TypeOfEquipment_prop.enumValueIndex;
        switch (st)
        {
            case TypesOfCards.Base:
                EditorGUILayout.PropertyField(SpellScriptName_prop);
                EditorGUILayout.PropertyField(SpecialSpellAmount_prop);
                EditorGUILayout.PropertyField(Targets_prop);
                EditorGUILayout.PropertyField(SpellAttribute_prop);
                break;
            case TypesOfCards.Tips:
                EditorGUILayout.PropertyField(SpellScriptName_prop);
                EditorGUILayout.PropertyField(SpecialSpellAmount_prop);
                EditorGUILayout.PropertyField(Targets_prop);
                EditorGUILayout.PropertyField(SpellAttribute_prop);
                EditorGUILayout.PropertyField(SubTypeOfTip_prop);
                break;
            case TypesOfCards.Equipment:
                EditorGUILayout.PropertyField(SpellScriptName_prop);
                EditorGUILayout.PropertyField(SpecialSpellAmount_prop);
                EditorGUILayout.PropertyField(Targets_prop);
                EditorGUILayout.PropertyField(SpellAttribute_prop);
                EditorGUILayout.PropertyField(TypeOfEquipment_prop);
                if (typeOfEquipment == TypeOfEquipment.Weapons)
                {
                    EditorGUILayout.PropertyField(WeaponAttackDistance_prop);
                }
                break;
        }
        EditorGUILayout.PropertyField(Suits_prop);
        EditorGUILayout.PropertyField(CardRank_prop);

        serializedObject.ApplyModifiedProperties();
    }

}
