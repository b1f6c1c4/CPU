`default_nettype none
module Input_debouncer(
   input Clock,
   input Reset,
   input load,
   input data,
   input btn,
   output reg Q
   );
   parameter div = 16'd25000;
   localparam dv = |div ? div - 16'd1 : 16'd0;

   reg [15:0] count;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         begin
            count <= dv;
            Q <= 1'b0;
         end
      else if (load)
         Q <= data;
      else if (Q ~^ btn)
         count <= dv;
      else
         begin
            count <= |count ? count - 16'b1 : 16'b0;
            if (~|count)
               Q <= ~Q;
         end

endmodule
