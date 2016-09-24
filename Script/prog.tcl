set hws [get_hardware_names]
set hw  [lindex $hws 0]
set dvs [get_device_names -hardware_name $hw]
set dv  [lindex $dvs 0]

begin_memory_edit -hardware_name $hw -device_name $dv

update_content_to_memory_from_file -instance_index 0 -mem_file_path "prog.hex" -mem_file_type hex

end_memory_edit
