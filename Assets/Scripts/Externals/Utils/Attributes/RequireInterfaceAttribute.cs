using UnityEngine;

namespace Utils.Attributes
{
    //срисовано отсюда: https://www.patrykgalach.com/2020/01/27/assigning-interface-in-unity-inspector/
    /// <summary>
    /// Attribute that require implementation of the provided interface.
    /// </summary>
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        // Interface type.
        public System.Type RequiredType { get; private set; }


        /// <summary>
        /// Requiring implementation of the <see cref="T:RequireInterfaceAttribute"/> interface.
        /// </summary>
        /// <param name="type">Interface type.</param>
        public RequireInterfaceAttribute(System.Type type)
        {
           RequiredType = type;
        }
    }
}