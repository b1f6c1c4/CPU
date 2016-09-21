`default_nettype none
`timescale 10ns/1ps
module ALU_tb;
`include "ALU_INTERFACE.v"

   reg [7:0] data_a;
   reg [7:0] data_b;
   reg carry_in;
   reg [AC_N-1:0] CS;
   wire [7:0] S;
   wire zero;
   wire carry_out;

   ALU mdl(CS, data_a, data_b, carry_in, S, zero, carry_out);

   initial
      begin
         CS = AC_ADX;

         data_a = 8'd212;
         data_b = 8'd44;
         carry_in = 1'b0;
         #4 $display("%d + %d = %d (%b) (%b)", data_a, data_b, S, zero, carry_out);
         data_a = 8'd42;
         data_b = 8'd67;
         carry_in = 1'b1;
         #4 $display("%d + %d = %d (%b) (%b)", data_a, data_b, S, zero, carry_out);
         data_a = 8'd1;
         data_b = 8'd77;
         carry_in = 1'b0;
         #4 $display("%d + %d = %d (%b) (%b)", data_a, data_b, S, zero, carry_out);
         data_a = 8'd9;
         data_b = 8'd64;
         carry_in = 1'b1;
         #4 $display("%d + %d = %d (%b) (%b)", data_a, data_b, S, zero, carry_out);

         CS = AC_SBX;

         data_a = 8'd212;
         data_b = 8'd44;
         carry_in = 1'b0;
         #4 $display("%d - %d = %d (%b) (%b)", data_a, data_b, S, zero, carry_out);
         data_a = 8'd42;
         data_b = 8'd67;
         carry_in = 1'b1;
         #4 $display("%d - %d = %d (%b) (%b)", data_a, data_b, S, zero, carry_out);
         data_a = 8'd1;
         data_b = 8'd77;
         carry_in = 1'b0;
         #4 $display("%d - %d = %d (%b) (%b)", data_a, data_b, S, zero, carry_out);
         data_a = 8'd9;
         data_b = 8'd64;
         carry_in = 1'b1;
         #4 $display("%d - %d = %d (%b) (%b)", data_a, data_b, S, zero, carry_out);

         CS = AC_AD;

         data_a = 8'd212;
         data_b = 8'd44;
         carry_in = 1'b0;
         #4 $display("%d + %d + %b = %d (%b) (%b)", data_a, data_b, carry_in, S, zero, carry_out);
         data_a = 8'd42;
         data_b = 8'd67;
         carry_in = 1'b1;
         #4 $display("%d + %d + %b = %d (%b) (%b)", data_a, data_b, carry_in, S, zero, carry_out);
         data_a = 8'd1;
         data_b = 8'd77;
         carry_in = 1'b0;
         #4 $display("%d + %d + %b = %d (%b) (%b)", data_a, data_b, carry_in, S, zero, carry_out);
         data_a = 8'd9;
         data_b = 8'd64;
         carry_in = 1'b1;
         #4 $display("%d + %d + %b = %d (%b) (%b)", data_a, data_b, carry_in, S, zero, carry_out);

         CS = AC_SB;

         data_a = 8'd212;
         data_b = 8'd44;
         carry_in = 1'b0;
         #4 $display("%d - %d - 1 + %b = %d (%b) (%b)", data_a, data_b, carry_in, S, zero, carry_out);
         data_a = 8'd42;
         data_b = 8'd67;
         carry_in = 1'b1;
         #4 $display("%d - %d - 1 + %b = %d (%b) (%b)", data_a, data_b, carry_in, S, zero, carry_out);
         data_a = 8'd1;
         data_b = 8'd77;
         carry_in = 1'b0;
         #4 $display("%d - %d - 1 + %b = %d (%b) (%b)", data_a, data_b, carry_in, S, zero, carry_out);
         data_a = 8'd9;
         data_b = 8'd64;
         carry_in = 1'b1;
         #4 $display("%d - %d - 1 + %b = %d (%b) (%b)", data_a, data_b, carry_in, S, zero, carry_out);

         CS = AC_AN;
         carry_in = 1'b0;

         data_a = 8'd212;
         data_b = 8'd44;
         #4 $display("%b & %b = %b (%b)", data_a, data_b, S, zero);
         data_a = 8'd42;
         data_b = 8'd67;
         #4 $display("%b & %b = %b (%b)", data_a, data_b, S, zero);
         data_a = 8'd1;
         data_b = 8'd77;
         #4 $display("%b & %b = %b (%b)", data_a, data_b, S, zero);
         data_a = 8'd9;
         data_b = 8'd64;
         #4 $display("%b & %b = %b (%b)", data_a, data_b, S, zero);

         CS = AC_OR;
         carry_in = 1'b0;

         data_a = 8'd212;
         data_b = 8'd44;
         #4 $display("%b | %b = %b (%b)", data_a, data_b, S, zero);
         data_a = 8'd42;
         data_b = 8'd67;
         #4 $display("%b | %b = %b (%b)", data_a, data_b, S, zero);
         data_a = 8'd1;
         data_b = 8'd77;
         #4 $display("%b | %b = %b (%b)", data_a, data_b, S, zero);
         data_a = 8'd9;
         data_b = 8'd64;
         #4 $display("%b | %b = %b (%b)", data_a, data_b, S, zero);

         CS = AC_LS;
         data_a = 8'd212;
         data_b = 8'd44;
         #4 $display("%d < %d = %d (%b)", data_a, data_b, S, zero);
         data_a = 8'd42;
         data_b = 8'd67;
         #4 $display("%d < %d = %d (%b)", data_a, data_b, S, zero);
         data_a = 8'd1;
         data_b = 8'd77;
         #4 $display("%d < %d = %d (%b)", data_a, data_b, S, zero);
         data_a = 8'd9;
         data_b = 8'd64;
         #4 $display("%d < %d = %d (%b)", data_a, data_b, S, zero);

         $finish;
      end
endmodule
