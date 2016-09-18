`default_nettype none
module Input_diff(
	input Clock,
	input Reset,
   input btn,
   output pressed
   );

   reg old;

   assign pressed = ~old && btn;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         old <= 1'b0;
      else
         old <= btn;

endmodule
