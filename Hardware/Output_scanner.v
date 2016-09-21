`default_nettype none
module Output_scanner(
   input Clock,
   input Reset,
   input [7:0] oct0,
   input [7:0] oct1,
   input [7:0] oct2,
   input [7:0] oct3,
   output reg [3:0] SD,
   output reg [7:0] SEG
   );
`ifdef SIMULATION
   parameter div = 1;
`else
   parameter div = 50000;
`endif

   reg [1:0] state;

   wire dv;
   clock_divider #(div) clk_div(Clock, Reset, dv);

   always @(*)
      if (~Reset)
         begin
            SD <= 4'b1111;
            SEG <= 8'b00000000;
         end
      else
         case (state)
            2'd0:
               begin
                  SD <= 4'b0001;
                  SEG <= oct0;
               end
            2'd1:
               begin
                  SD <= 4'b0010;
                  SEG <= oct1;
               end
            2'd2:
               begin
                  SD <= 4'b0100;
                  SEG <= oct2;
               end
            2'd3:
               begin
                  SD <= 4'b1000;
                  SEG <= oct3;
               end
         endcase

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         state <= 2'd0;
      else if (dv)
         state <= state + 2'd1;

endmodule
