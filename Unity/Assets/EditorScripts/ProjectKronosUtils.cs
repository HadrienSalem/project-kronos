using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
// ReSharper disable All

namespace ProjectKronosUtils
{
    static class ExtensionMethods
    {
        public static void SetParentGameObject(this GameObject child, GameObject parent)
        {
            child.transform.SetParent(parent.transform);
        }

        public static Collider2D TopMostCollider(this RaycastHit2D[] hits)
        {
            if (hits!=null && hits.Length > 0)
            {
                Array.Sort(hits, delegate (RaycastHit2D hit1, RaycastHit2D hit2)
                {
                    return hit1.collider.gameObject.layer.CompareTo(hit2.collider.gameObject.layer);
                });
                return hits[0].collider;
            }
            else return null;

        }
    }

    static class EditorUtils
    {
        public static GameObject PrefabField(string label, GameObject item)
        {
            GameObject result;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(label);
            result = (GameObject)EditorGUILayout.ObjectField(item, typeof(GameObject), false);
            EditorGUILayout.EndHorizontal();

            return result;
        }

        public static int IntField(string label, int item)
        {
            int result;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(label);
            result = EditorGUILayout.IntField(item);
            EditorGUILayout.EndHorizontal();

            return result;
        }

        public static void MakeButton(String label, float height, Action action)
        {
            if (GUILayout.Button(label, GUILayout.Height(height)))
            {
                action();
            }
        }
    }

    static class MouseUtils
    {
        public static void HandleHoverObject(int targetLayer, Action<Collider2D> actionIfHit, Action<Collider2D> actionIfNoHit)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            Collider2D hit = Physics2D.RaycastAll(mousePos2D, Vector2.zero, Mathf.Infinity).TopMostCollider();
            
            if (hit != null && hit.gameObject.layer == targetLayer)
            {
                actionIfHit(hit);
            }
            else
            {
                actionIfNoHit(hit);
            }
        }
        
        public static void HandleClickObject(int targetLayer, Action<Collider2D> actionIfHit, Action<Collider2D> actionIfNoHit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                Collider2D hit = Physics2D.RaycastAll(mousePos2D, Vector2.zero, Mathf.Infinity).TopMostCollider();
            
                if (hit != null && hit.gameObject.layer == targetLayer)
                {
                    actionIfHit(hit);
                }
                else
                {
                    actionIfNoHit(hit);
                }
            }

        }
    }
}
