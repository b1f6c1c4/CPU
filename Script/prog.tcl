begin_memory_edit -hardware_name "USB-Blaster \[USB-0\]" -device_name "@1: EP2C5 (0x020B10DD)"

update_content_to_memory_from_file -instance_index 0 -mem_file_path "prog.hex" -mem_file_type hex

end_memory_edit
