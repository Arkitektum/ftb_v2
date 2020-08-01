Add more info about the library's limitations regarding responsibility

This library orchestrates and contains operations for the download and persistence logic of what is available on the download queue.

The process is as follows:
	- Get items from download queue for the configured service codes
	- Persists the metadata to a store of some sort
	- Sends a message to some messaging service for other components to continue processing