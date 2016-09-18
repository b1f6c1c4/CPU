`default_nettype none
module Input_keyboard(
   input Clock,
   input Reset,
   input [3:0] H,
   inout [3:0] V,
   output reg [15:0] res
   );
`ifdef SIMULATION
   parameter div = 8;
`else
   parameter div = 100000;
`endif

   reg [1:0] state;

   assign V[0] = Reset && state == 2'd0 ? 1'b1 : 1'bz;
   assign V[1] = Reset && state == 2'd1 ? 1'b1 : 1'bz;
   assign V[2] = Reset && state == 2'd2 ? 1'b1 : 1'bz;
   assign V[3] = Reset && state == 2'd3 ? 1'b1 : 1'bz;

   wire dv;
   clock_divider #(div) clk_scan(Clock, Reset, dv);

   wire [3:0] debD;
   wire [3:0] debQ;

   Input_debouncer #(div/100) db0(.Clock(Clock), .Reset(Reset), .load(dv),
                                  .data(debD[0]), .Q(debQ[0]), .btn(H[0]));
   Input_debouncer #(div/100) db1(.Clock(Clock), .Reset(Reset), .load(dv),
                                  .data(debD[1]), .Q(debQ[1]), .btn(H[1]));
   Input_debouncer #(div/100) db2(.Clock(Clock), .Reset(Reset), .load(dv),
                                  .data(debD[2]), .Q(debQ[2]), .btn(H[2]));
   Input_debouncer #(div/100) db3(.Clock(Clock), .Reset(Reset), .load(dv),
                                  .data(debD[3]), .Q(debQ[3]), .btn(H[3]));

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         state <= 2'd0;
      else if (dv)
         state <= state + 2'd1;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         res <= 16'b0;
      else if (dv)
         case (state)
            2'd0: res[0+:4] <= debQ;
            2'd1: res[4+:4] <= debQ;
            2'd2: res[8+:4] <= debQ;
            2'd3: res[12+:4] <= debQ;
         endcase

endmodule
