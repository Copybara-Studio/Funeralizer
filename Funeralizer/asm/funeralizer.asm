;INCLUDE C:\masm32\include\windows.inc
.CODE

funeralize_asm PROC
	; Save the registers we will be using so we can restore them later. The program will crash if we don't.
	mov    [RSP + 8], RCX
    push   R15
    push   R14
    push   R13
    sub    RSP, 64
	; Move the array pointer into the r15 register.
	mov r15, rcx
	; Multiply the length by 4 (offset) and store the result in the r14 register. We will use this to loop through the array.
	mov rax, 4
	mul rdx
	mov r14, rax
	
	; Clear the rsi register.
	xor r13, r13

	; Beginning of loop instruction block.
@loop:
	
	; Deal the R component, multiply by weight and store the result in the array.
	mov eax, [r15+r13]
	mov ecx, 299
	mul ecx
	mov [r15+r13], eax
	
	; Deal the G component, multiply by weight and store the result in the array.
	mov eax, [r15+r13+4]
	mov ecx, 587
	mul ecx
	mov [r15+r13+4], eax
	
	; Deal the B component, multiply by weight and store the result in the array.
	mov eax, [r15+r13+8]
	mov ecx, 114
	mul ecx
	mov [r15+r13+8], eax

	; Add the components together.
	mov eax, [r15+r13]
	add eax, [r15+r13+4]
	add eax, [r15+r13+8]

	; Divide by 1000. The result should be between 0 and 255.
	mov ecx, 1000
	div ecx

	; Replace the components with the brightness.
	mov [r15+r13], eax
	mov [r15+r13+4], eax
	mov [r15+r13+8], eax

	; Prepare for the next iteration.
	add r13, 12
	cmp r13, r14
	jc @loop
	
	; Restore the registers we used.
	add      RSP, 64
    pop      R13
    pop      R14
    pop      R15
	; Return from procedure.
	ret
funeralize_asm ENDP

END