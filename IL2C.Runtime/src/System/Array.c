#include "il2c_private.h"

/////////////////////////////////////////////////////////////
// System.Array

int32_t System_Array_getLength(System_Array* this__)
{
    il2c_assert(this__ != NULL);
    il2c_assert(this__->vptr0__ == &System_Array_VTABLE__);
    il2c_assert(this__->elementType__ != NULL);
    il2c_assert(this__->Length >= 0);

    return (int32_t)(this__->Length);
}

int32_t System_Array_GetLowerBound(System_Array* this__, int32_t dimension)
{
    il2c_assert(this__ != NULL);
    il2c_assert(this__->vptr0__ == &System_Array_VTABLE__);
    il2c_assert(this__->elementType__ != NULL);
    il2c_assert(this__->Length >= 0);

    // TODO: MD array not implemented.
    if (dimension != 0)
    {
        il2c_throw_indexoutofrangeexception__();
    }

    // TODO: bind range not implemented.
    return 0;
}

int32_t System_Array_GetUpperBound(System_Array* this__, int32_t dimension)
{
    il2c_assert(this__ != NULL);
    il2c_assert(this__->vptr0__ == &System_Array_VTABLE__);
    il2c_assert(this__->elementType__ != NULL);
    il2c_assert(this__->Length >= 0);

    // TODO: MD array not implemented.
    if (dimension != 0)
    {
        il2c_throw_indexoutofrangeexception__();
    }

    // TODO: bind range not implemented.
    return (int32_t)(this__->Length - 1);
}

/////////////////////////////////////////////////
// Array special functions

#if defined(IL2C_USE_LINE_INFORMATION)
System_Array* il2c_new_array__(
    IL2C_RUNTIME_TYPE elementType, intptr_t length, const char* pFile, int line)
#else
System_Array* il2c_new_array__(
    IL2C_RUNTIME_TYPE elementType, intptr_t length)
#endif
{
    il2c_assert(elementType != NULL);
    
    if (length < 0)
    {
        // throw OverflowException
        il2c_assert(0);
    }

    uintptr_t elementSize = il2c_sizeof__(elementType);
    il2c_assert(elementSize >= 1);

    // -1 is "uint8_t Item[1]"
    uintptr_t size = (uintptr_t)sizeof(System_Array) + ((uintptr_t)length) * elementSize;
    
#if defined(IL2C_USE_LINE_INFORMATION)
    System_Array* arr = il2c_get_uninitialized_object_internal__(
        il2c_typeof(System_Array), size, pFile, line);
#else
    System_Array* arr = il2c_get_uninitialized_object_internal__(
        il2c_typeof(System_Array), size);
#endif

    arr->vptr0__ = &System_Array_VTABLE__;

    arr->elementType__ = elementType;
    arr->Length = length;

    return arr;
}

/////////////////////////////////////////////////
// VTable and runtime type info declarations

static void System_Array_MarkHandler(System_Array* arr)
{
    il2c_assert(arr != NULL);
    il2c_assert(arr->vptr0__ == &System_Array_VTABLE__);

    if (arr->elementType__->flags & IL2C_TYPE_VALUE)
    {
        // TODO: value type requires marking if contains objref inside.
        return;
    }

    intptr_t index;
    for (index = 0; index < arr->Length; index++)
    {
        void* pReference = il2c_array_item(arr, void*, index);
        if (pReference != NULL)
        {
            il2c_default_mark_handler__(pReference);
        }
    }
}

IL2C_RUNTIME_TYPE_BEGIN(System_Array, "System.Array", IL2C_TYPE_VARIABLE, 0, System_Object, System_Array_MarkHandler, 0)
IL2C_RUNTIME_TYPE_END();
