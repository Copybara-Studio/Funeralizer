;-------------------------------------------------------------------------
.386
.MODEL FLAT, STDCALL

OPTION CASEMAP:NONE
;INCLUDE \masm32\include\windows.inc
.CODE

DllEntry PROC hInstDLL:DWORD, reason:DWORD, reserved1:DWORD

mov	eax, 1 	;TRUE
ret

DllEntry ENDP

funeralize_asm PROC contents: DWORD, length: DWORD

	; Clear the index register.
	xor esi, esi

	; Beginning of loop instruction block.
repeat:
	
	; Deal the R component.
	mov eax, contents[esi]
	mul 299
	div 1000
	mov contents[esi], eax

	; Prepare for the G component.
	; Check whether it is valid!
	inc esi
	cmp esi, length
	jge error
	
	; Deal the G component.
	mov eax, contents[esi]
	mul 587
	div 1000
	mov contents[esi], eax
	
	; Prepare for the B component.
	; Check whether it is valid!
	inc esi
	cmp esi, length
	jge error
	
	; Deal the B component.
	mov eax, contents[esi]
	mul 114
	div 1000
	mov contents[esi], eax

	inc esi
	cmp ecx, length
	jl repeat
	
	mov eax, 1; Success
	
error:
	mov eax, 0; Failure

funeralize_asm ENDP

END DllEntry
;-------------------------------------------------------------------------