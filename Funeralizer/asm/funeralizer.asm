;INCLUDE C:\masm32\include\windows.inc
.DATA
weightR dd 0.299
weightG dd 0.587
weightB dd 0.114

; ------------------------------

.CODE

funeralize_asm PROC
	; Save the registers we will be using so we can restore them later. The program will crash if we don't.
    push   R15
    push   R14
    push   R13
	push   R12
	push   R11
	push   R10
	push   R9
    
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
	; Load the weight factors into ymm1, ymm2, ymm3.
	movss xmm0, dword ptr [weightR]
	movss xmm1, dword ptr [weightG]
	movss xmm2, dword ptr [weightB]

	movzx r10, byte ptr [r15+r13]
	movzx r11, byte ptr [r15+r13+4]
	movzx r12, byte ptr [r15+r13+8]

	cvtsi2ss xmm3, r10  
    cvtsi2ss xmm4, r11  
    cvtsi2ss xmm5, r12  
	; Multiply the RGB components by the corresponding weight factors.
	mulss xmm3, xmm0   
    mulss xmm4, xmm1   
    mulss xmm5, xmm2

	; Add the results together.
	addss xmm3, xmm4   
    addss xmm3, xmm5
	
	; Convert the result to integer and store it back to memory.
	cvttss2si r9, xmm3
	mov byte ptr [r15 + r13], r9b
    mov byte ptr [r15 + r13 + 4], r9b
    mov byte ptr [r15 + r13 + 8], r9b

	; Prepare for the next iteration.
	add r13, 12
	cmp r13, r14
	jc @loop
	
	; Restore the registers we used.
	pop   R9
	pop   R10
	pop   R11
	pop   R12
	pop   R13
	pop   R14
	pop   R15
	; Return from procedure.
	ret

funeralize_asm ENDP

END
