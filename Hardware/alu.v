`default_nettype none
module alu(
   input [AC_N-1:0] CS,
   input [N-1:0] data_a,
   input [N-1:0] data_b,
   input carry_in,
   output reg [N-1:0] S,
   output zero,
   output reg carry_out
   );
   parameter N = 8;
`include "ALU_INTERFACE.v"

   assign zero = ~|S;

   always @(*)
      case (CS)
         AC_AD: {carry_out,S} <= data_a + data_b + carry_in;
         AC_SB: {carry_out,S} <= data_a - data_b - carry_in;
         AC_AN: begin S <= data_a & data_b; carry_out <= 1'b0; end
         AC_OR: begin S <= data_a | data_b; carry_out <= 1'b0; end
         AC_LS: begin S <= data_a < data_b; carry_out <= 1'b0; end
         default: begin S <= {N{1'bx}}; carry_out <= 1'b0; end
      endcase

endmodule
