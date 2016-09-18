`default_nettype none
module CPU(
   input CLK,
   input RST,
   input [3:0] H,
   output [3:0] V,
   output [3:0] SD,
   output [7:0] SEG,
   output [7:0] LD,
   input [7:0] SB,
   output Buzz
   );

   // fundamental modules
   wire Clock, Reset;
   assign Clock = CLK;
   rst_recover rst(Clock, RST, Reset);

   // links
   wire re_WE;
   wire [1:0] re_N1, re_N2, re_ND;
   wire [7:0] re_Q1, re_Q2, re_DI;

   wire io_RE, io_WE, io_read, io_write;
   wire [7:0] io_addr, io_Din, io_Dout;

   wire wren, ra_WR;
   wire [7:0] ra_addr, ra_data, ra_q;

   wire [7:0] ro_addr;
   wire [15:0] ro_q;

   wire cu_jump, cu_branch, cu_alusrcb, cu_writemem;
   wire cu_writereg, cu_memtoreg, cu_regdes, cu_wrflag;
   wire [2:0] cu_aluc;
   wire [3:0] cu_op;

   wire [7:0] pc_imm, pc_pc;

   wire al_zero, al_Cin, al_Cout;
   wire [7:0] al_A, al_B, al_S;

   wire in_finish;
   wire [2:0] in_op;
   wire [7:0] in_SRCH, in_SRCL, in_DSTH, in_DSTL;
   wire [7:0] ou_H, ou_L;

   // main modules
   reg4_8 re(
      .Clock(Clock), .Reset(Reset),
      .N1(re_N1), .Q1(re_Q1),
      .N2(re_N2), .Q2(re_Q2),
      .ND(re_ND), .DI(re_DI), .REG_WE(re_WE));

   IO_PORT io(
      .addr(io_addr), .RE(io_RE), .WE(io_WE),
      .Din(io_Din), .Dout(io_Dout),
      .io_read(io_read), .io_write(io_write),
      .IO0(ou_L), .IO1(ou_H),
      .IO2(in_SRCL), .IO3(in_SRCH),
      .IO4(in_DSTL), .IO5(in_DSTH),
      .IO6(in_op));

   lpm_ram_256_8 ra(
      .Clock(Clock),
      .address(ra_addr), .data(ra_data),
      .wren(ra_WR), .q(ra_q));

   lpm_rom_256_16 ro(
      .Clock(Clock),
      .address(ro_addr), .q(ro_q));

   ctrlunit cu(
      .OP(cu_op), .ZERO(al_zero),
      .JUMP(cu_jump), .BRANCH(cu_branch),
      .ALUC(cu_aluc), .WRITEMEM(cu_writemem),
      .WRITEREG(cu_writereg), .MEMTOREG(cu_memtoreg),
      .REGDES(cu_regdes), .WRFLAG(cu_wrflag));

   instrconunit pc(
      .Clock(Clock), .Reset(Reset),
      .BRANCH(cu_branch), .JUMP(cu_jump),
      .imm(pc_imm), .PC(pc_pc));

   alu alu(
      .CS(cu_aluc), .data_a(al_A), .data_b(al_B), .carry_in(al_Cin),
      .S(al_S), .zero(al_zero), .carry_out(al_Cout));

   key_scan in(
      .CLK(Clock), .RESET(Reset),
      .V1(H[3]), .V2(H[2]), .V3(H[1]), .V4(H[0]),
      .H1(V[3]), .H2(V[2]), .H3(V[1]), .H4(V[0]),
      .SRCH(in_SRCH), .SRCL(in_SRCL), .DSTH(in_DSTH), .DSTL(in_DSTL),
      .ALU_OP(in_op), .finish(in_finish));

   seg out(
      .CLK_seg(Clock),
      .data_inH(ou_H), .data_inL(ou_L),
      .seg_sel(SD), .data_out(SEG));

endmodule
