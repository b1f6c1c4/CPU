`default_nettype none
module Input_buffer(
   input Clock,
   input Reset,
   input [N-1:0] in_cmd,
   input ack,
   output reg [N-1:0] out_cmd
   );
   parameter N = 8;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         out_cmd <= {N{1'b1}};
      else if (ack || ~&in_cmd)
         out_cmd <= in_cmd;

endmodule
