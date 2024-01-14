;INCLUDE C:\masm32\include\windows.inc
.CODE

funeralize_asm PROC
	; Save the registers we will be using so we can restore them later. The program will crash if we don't.
	push rsi
	push rdi
	push r10
	; Multiply the length by 4 (offset) and store the result in the rax register. We will use this to loop through the array.
	mov rax, 4
	mul rdx
	; Move the length into the r10 register.
	mov r10, rax
	; Move the array pointer into the rdi register.
	mov rdi, rcx
	; Clear the rsi register.
	xor rsi, rsi

	; Beginning of loop instruction block.
@loop:
	
	; Deal the R component, multiply by weight and store the result in the array.
	mov eax, [rdi+rsi]
	mov ecx, 299
	mul ecx
	mov [rdi+rsi], eax
	
	; Deal the G component, multiply by weight and store the result in the array.
	mov eax, [rdi+rsi+4]
	mov ecx, 587
	mul ecx
	mov [rdi+rsi+4], eax
	
	; Deal the B component, multiply by weight and store the result in the array.
	mov eax, [rdi+rsi+8]
	mov ecx, 114
	mul ecx
	mov [rdi+rsi+8], eax

	; Add the components together.
	mov eax, [rdi+rsi]
	add eax, [rdi+rsi+4]
	add eax, [rdi+rsi+8]

	; Divide by 1000. The result should be between 0 and 255.
	mov ecx, 1000
	div ecx

	; Replace the components with the brightness.
	mov [rdi+rsi], eax
	mov [rdi+rsi+4], eax
	mov [rdi+rsi+8], eax

	; Prepare for the next iteration.
	add rsi, 12
	cmp rsi, r10
	jc @loop
	
	; Restore the registers we used.
	pop r10
	pop rdi
	pop rsi
	; Return from procedure.
	ret
funeralize_asm ENDP

END