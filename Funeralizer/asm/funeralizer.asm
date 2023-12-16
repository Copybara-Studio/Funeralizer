;INCLUDE C:\masm32\include\windows.inc
.CODE

funeralize_asm PROC contents: QWORD, len: QWORD

	; Clear the index register.
	xor rsi, rsi
	mov rdi, contents
	; Beginning of loop instruction block.
@loop:
	
	; Deal the R component.
	
	mov eax, [rdi+rsi]
	mov ecx, 299
	mul ecx
	mov ebx, 1000
	div ebx
	mov [rdi+rsi], eax

	; Prepare for the G component.
	; Check whether it is valid!
	mov rax, rsi
	add rax, 1
	cmp rax, len
	jnc @error
	
	; Deal the G component.
	mov eax, [rdi+rsi+1]
	mov ecx, 587
	mul ecx
	mov ebx, 1000
	div ebx
	mov [rdi+rsi], eax
	
	; Prepare for the B component.
	; Check whether it is valid!
	mov rax, rsi
	add rax, 2
	cmp rax, len
	jnc @error
	
	; Deal the B component.
	mov eax, [rdi+rsi+2]
	mov ecx, 114
	mul ecx
	mov ebx, 1000
	div ebx
	mov [rdi+rsi], eax

	; Add the components together.
	mov eax, [rdi+rsi]
	add eax, [rdi+rsi+1]
	add eax, [rdi+rsi+2]

	; Replace the components with the brightness.
	mov [rdi+rsi], eax
	mov [rdi+rsi+1], eax
	mov [rdi+rsi+2], eax

	; Prepare for the next iteration.
	add rsi, 3
	cmp rsi, len
	jc @loop
	
	mov eax, 1; Success
	ret
@error:
	mov eax, 0; Failure
	ret
funeralize_asm ENDP

END