// using UnityEngine;
//
// public class Interaction : MonoBehaviour
// {
//     private void OnTriggerEnter(Collider other)
//     {
//         Item item = other.gameObject.GetComponent<Item>();
//         if (item != null)
//         {
//             item.Use();
//         }
//     } 
//     
//     private void OnTriggerExit(Collider other)
//     {
//         Item item = other.gameObject.GetComponent<Item>();
//         if (item != null)
//         {
//             item.ExitUse();
//         }
//     }
// }