;INCLUDE C:\masm32\include\windows.inc
.CODE

funeralize_asm PROC contents: DWORD, len: DWORD

	; Clear the index register.
	xor esi, esi

	; Beginning of loop instruction block.
@loop:
	
	; Deal the R component.
	mov eax, contents[esi]
	mov ecx, 299
	mul ecx
	mov edx, 1000
	div edx
	mov contents[esi], eax

	; Prepare for the G component.
	; Check whether it is valid!
	inc esi
	cmp esi, len
	jnc @error
	
	; Deal the G component.
	mov eax, contents[esi]
	mov ecx, 587
	mul ecx
	mov edx, 1000
	div edx
	mov contents[esi], eax
	
	; Prepare for the B component.
	; Check whether it is valid!
	inc esi
	cmp esi, len
	jnc @error
	
	; Deal the B component.
	mov eax, contents[esi]
	mov ecx, 114
	mul ecx
	mov edx, 1000
	div edx
	mov contents[esi], eax

	inc esi
	cmp esi, len
	jc @loop
	
	mov eax, 1; Success
	ret
@error:
	mov eax, 0; Failure
	ret
funeralize_asm ENDP

END