/*
  Script OrgInfo.fs

  Author: Bomitali Evelino
  Tested with versions: 5.0

  Defines a structure that contains the information needed to create an element
 */

// The following properties should be
// set on the object:
//
// name : The name of the new organization.
// parent : DName of the parent org.
// children : new Array() consisting of the DNames of all elements "in" the org or an empty array
// clazz : A string representing the class of the new org
// DisplaySourceElements: Boolean, indicating whether the elements should be displayed for this org if possible.

// Possible properties name
// script:
// matches:
// rollup: (or Algorithm ?)
// rollupParameters: (or AlgorithmParameters ?)

function OrgInfo (clazz, name, parent, children, displaySourceElements, props) {
    if (!orgInfo.clazz) {
        return "clazz must be defined";
    }
    if (!orgInfo.name) {
        return "name must be specified";
    }

    if (!orgInfo.parent) {
        return "parent must be specified";
    }
    if (!orgInfo.children) {
        return "children must be specified (may be an empty array)";
    }
    if (!orgInfo.displaySourceElements) {
        return "displaySourceElements must be specified";
    }
    if (!orgInfo.props) {
        return "props must be specified (may be empty)";
    }
}

